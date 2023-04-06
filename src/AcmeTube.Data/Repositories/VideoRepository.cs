using AcmeTube.Application.Repositories;
using AcmeTube.Data.Contexts;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Data.Repositories
{
	public sealed class VideoRepository : Repository<Video>, IVideoRepository
	{
		public VideoRepository(MainContext context) : base(context) { }

		private DbSet<VideoComment> CommentDbSet => Context.Set<VideoComment>();

		public Task<PaginatedResult<Video>> ListPaginatedByFilterAsync(VideoFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken)
		{
			var query = DbSetAsNoTracking;

			return base.ListPaginatedAsync(query, pagingParameters, cancellationToken);
		}

		public Task<bool> ExistsAsync(string id, CancellationToken cancellationToken) =>
			ExistsByExpressionAsync(it => it.Id == id, cancellationToken);

		public override Task CreateAsync(Video video, CancellationToken cancellationToken)
		{
			Context.Entry(video.Channel).State = EntityState.Unchanged;

			return base.CreateAsync(video, cancellationToken);
		}

		public Task DeleteAsync(Video video, CancellationToken cancellationToken) =>
			DbSet.Where(it => it.Id == video.Id).ExecuteDeleteAsync(cancellationToken);

		public Task<VideoRatingType?> GetRatingVideoAsync(string videoId, string membershipId, CancellationToken cancellationToken) =>
			base.ExecuteScalarWithTransactionAsync<VideoRatingType?>(
				commandText: "select type from video_rating where video_id = @VideoId and membership_id = @MembershipId",
				parameters: new { VideoId = videoId, MembershipId = membershipId },
				cancellationToken: cancellationToken);
		
		public Task UpsertRatingVideoAsync(string videoId, string membershipId, VideoRatingType ratingType, CancellationToken cancellationToken)
		{
			const string commandText = @"
				MERGE INTO video_rating target
				USING (VALUES (@VideoId, @MembershipId, @RatingType, current_timestamp)) source (video_id, membership_id, type, created_at) 
				ON target.video_id = source.video_id and target.membership_id = source.membership_id
				WHEN MATCHED THEN
				  UPDATE set type = source.type
  					   , created_at = source.created_at
				WHEN NOT MATCHED THEN
				  INSERT (video_id, membership_id, type, created_at)
				  VALUES (source.video_id, source.membership_id, source.type, source.created_at);";

			return base.ExecuteWithTransactionAsync(commandText, new { videoId, membershipId, RatingType = ratingType }, cancellationToken);
		}

		public async Task<bool> IncreaseVideoViewsCountAsync(string videoId, string membershipId, CancellationToken cancellationToken)
		{
			var commandText = @"
				INSERT INTO video_view (video_id, membership_id, created_at)
					VALUES (@VideoId, @MembershipId, current_timestamp)
				ON CONFLICT ON CONSTRAINT video_view_pk DO NOTHING;";

			return await base.ExecuteWithTransactionAsync(commandText, new { videoId, membershipId }, cancellationToken) == 1;
		}

		public async Task<bool> DeleteRatingVideoAsync(string videoId, string membershipId, CancellationToken cancellationToken)
		{
			const string commandText = "DELETE FROM video_rating WHERE video_id = @VideoId and membership_id = @MembershipId";

			return await base.ExecuteWithTransactionAsync(commandText, new { videoId, membershipId }, cancellationToken) == 1;
		}

		/// <inheritdoc />
		public async Task<VideoComment> GetCommentByIdAsync(string id, CancellationToken cancellationToken) =>
			await CommentDbSet.FindAsync(new object[] { id }, cancellationToken);

		public Task<PaginatedResult<VideoComment>> ListCommentsPaginatedByFilterAsync(string videoId, PagingParameters pagingParameters, CancellationToken cancellationToken)
		{
			var query = CommentDbSet.Where(it => it.Video.Id == videoId);

			return base.ListPaginatedAsync(query, pagingParameters, cancellationToken);
		}

		public async Task CreateCommentAsync(VideoComment comment, CancellationToken cancellationToken) => 
			await CommentDbSet.AddAsync(comment, cancellationToken);

		public async Task<bool> DeleteCommentAsync(int commentId, CancellationToken cancellationToken) =>
			await CommentDbSet.Where(it => it.Id == commentId).ExecuteDeleteAsync(cancellationToken) == 1; 
	}
}
