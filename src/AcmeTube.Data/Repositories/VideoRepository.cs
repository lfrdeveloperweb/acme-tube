using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Repositories;
using AcmeTube.Data.Contexts;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;
using AcmeTube.Domain.Security;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AcmeTube.Data.Repositories
{
    public sealed class VideoRepository : Repository<Video>, IVideoRepository
    {
        private const string SplitOn = "id,tags,id,id";

        private const string BaseSelectCommandText = @"
            SELECT t.video_id as id
                 , t.title
                 , t.description
                 , t.channel_id
                 , t.priority
                 , t.due_date
                 , t.completed_at
                 , t.created_at
                 , t.updated_at
                 , t.deleted_at
                 , t.tags
                 , t.created_by as id
                 , '' as name
                 , t.updated_by as id
                 , '' as name
              FROM video t";

        private const string CommentBaseSelectCommandText = @"
            SELECT c.video_comment_id as id
                 , c.video_id
                 , c.description
                 , c.created_at
                 , c.created_by as id
                 , '' as name
              FROM video_comment c";

        public VideoRepository(MainContext context) : base(context) { }

        /// <inheritdoc />
        public async Task<Video> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await base.DbSetAsNoTracking.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);


            const string commandText = $"{BaseSelectCommandText} WHERE t.video_id = @Id";

            var query = await base.Connection.QueryAsync<Video, string, Membership, Membership, Video>( 
                sql: commandText,
                map: MapProperties,
                param: new { Id = id },
                transaction: base.Transaction,
                splitOn: SplitOn);

            return query.FirstOrDefault();
        }
        
        public async Task<PaginatedResult<Video>> ListPaginatedByFilterAsync(VideoFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken)
        {
            var commandText = new StringBuilder($@"
               SELECT COUNT(t.video_id)
                 FROM video t @DynamicFilter;

              {BaseSelectCommandText} @DynamicFilter
               ORDER BY t.created_at
                 OFFSET @Offset ROWS
             FETCH NEXT @RecordsPerPage ROWS ONLY;");

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { Offset = pagingParameters.Offset, RecordsPerPage = pagingParameters.RecordsPerPage });

            ApplyFilter(commandText, filter, parameters);

            using var multiQuery = await base.Connection.QueryMultipleAsync(
                new CommandDefinition(commandText.ToString(), parameters, Transaction, cancellationToken: cancellationToken));

            var totalRecords = await multiQuery.ReadSingleAsync<int>();
            var data = multiQuery.Read<Video, string, Membership, Membership, Video>(MapProperties, splitOn: SplitOn);

            return new PaginatedResult<Video>(data.ToList(), totalRecords);
        }

        public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken)
        {
            const string commandText =
                @"SELECT 1 FROM video WHERE video_id = @Id;";

            return ExistsWithTransactionAsync(commandText, new { Id = id }, cancellationToken);
        }

        public Task CreateAsync(Video video, CancellationToken cancellationToken)
        {
             base.DbSet.Add(video);

             return Task.CompletedTask;
        }

        public Task UpdateAsync(Video video, CancellationToken cancellationToken)
        {
            const string commandText = @"
                UPDATE video
                   SET title = @Title
                     , description = @Description
                     , channel_id = @ChannelId
                     , priority = @Priority
                     , due_date = @DueDate
                     , tags = @Labels
                     , updated_by = @UpdatedBy
                     , updated_at = @UpdatedAt
                 WHERE video_id = @Id;";

            return ExecuteWithTransactionAsync(commandText, new
            {
	            video.Id,
                video.Title,
                video.Description,
                ChannelId = video.Channel?.Id,
                Labels = JsonConvert.SerializeObject(video.Tags, Formatting.None),
                video.UpdatedAt,
                video.UpdatedBy
            }, cancellationToken);
        }

        public Task DeleteAsync(Video video, CancellationToken cancellationToken)
        {
            const string commandText = @"
                UPDATE video
                   SET deleted_at = CURRENT_TIMESTAMP
                 WHERE video_id = @Id;";

            return ExecuteWithTransactionAsync(commandText, cancellationToken, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<VideoComment> GetCommentByIdAsync(string id, CancellationToken cancellationToken)
        {
            const string commandText = $"{CommentBaseSelectCommandText} WHERE c.video_comment_id = @Id";

            var query = await base.Connection.QueryAsync<VideoComment, Membership, VideoComment>(
                sql: commandText,
                map: MapProperties,
                param: new { Id = id },
                transaction: base.Transaction,
                splitOn: SplitOn);

            return query.FirstOrDefault();
        }

        public async Task<PaginatedResult<VideoComment>> ListCommentsPaginatedByFilterAsync(VideoCommentFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken)
        {
            var commandText = new StringBuilder($@"
               SELECT COUNT(c.video_id)
                 FROM video_comment c @DynamicFilter;

              {CommentBaseSelectCommandText} @DynamicFilter
               ORDER BY c.created_at
                 OFFSET @Offset ROWS
             FETCH NEXT @RecordsPerPage ROWS ONLY;");

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(new { Offset = pagingParameters.Offset, RecordsPerPage = pagingParameters.RecordsPerPage });

            CommentApplyFilter(commandText, filter, parameters);

            using var multiQuery = await base.Connection.QueryMultipleAsync(
                new CommandDefinition(commandText.ToString(), parameters, Transaction, cancellationToken: cancellationToken));

            var totalRecords = await multiQuery.ReadSingleAsync<int>();
            var data = multiQuery.Read<VideoComment>();

            return new PaginatedResult<VideoComment>(data.ToList(), totalRecords);
        }

        public async Task CreateCommentAsync(VideoComment comment, CancellationToken cancellationToken)
        {
            const string commandText = @"
                INSERT INTO video_comment
                (
                    video_id,
                    description,
                    created_at,
                    created_by
                ) 
                VALUES 
                (
                    @videoId,
                    @Description,
                    @CreatedAt,
                    @CreatedBy
                )
                RETURNING video_comment_id;";

            comment.Id = await ExecuteScalarWithTransactionAsync<int>(commandText, new
            {
                Id = comment.Id,
                videoId = comment.VideoId,
                Description = comment.Description,
                CreatedAt = comment.CreatedAt,
                CreatedBy = comment.CreatedBy?.Id
            }, cancellationToken);
        }

        public Task DeleteCommentAsync(VideoComment comment, CancellationToken cancellationToken)
        {
            const string commandText = "DELETE FROM video_comment WHERE video_comment_id = @Id;";

            return ExecuteWithTransactionAsync(commandText, new { Id = comment.Id }, cancellationToken);
        }

        private static void ApplyFilter(StringBuilder sql, VideoFilter filter, DynamicParameters parameters)
        {
            var conditions = new Collection<string>();

            if (filter.IsCompleted.HasValue)
            {
                conditions.Add($"t.completed_at is {(filter.IsCompleted.Value ? "not null" : "null")}");
            }

            // Put everything together in the WHERE clause
            var dynamicFilter = conditions.Any() ? $" WHERE {string.Join(" AND ", conditions)}" : "";

            sql.Replace("@DynamicFilter", dynamicFilter);
        }

        private static void CommentApplyFilter(StringBuilder sql, VideoCommentFilter filter, DynamicParameters parameters)
        {
            var conditions = new Collection<string>();

            conditions.Add("c.video_id = @VideoId");
            parameters.Add("videoId", filter.VideoId);

            // Put everything together in the WHERE clause
            var dynamicFilter = conditions.Any() ? $" WHERE {string.Join(" AND ", conditions)}" : "";

            sql.Replace("@DynamicFilter", dynamicFilter);
        }

        private static Video MapProperties(Video video, string tags, Membership creator, Membership updater)
        {
            video.Tags = JsonConvert.DeserializeObject<ICollection<string>>(tags);

            return video;
        }

        private static VideoComment MapProperties(VideoComment comment, Membership creator)
        {
            comment.CreatedBy = creator;

            return comment;
        }
    }
}
