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

		public Task DeleteAsync(Video video, CancellationToken cancellationToken) =>
			DbSet.Where(it => it.Id == video.Id).ExecuteDeleteAsync(cancellationToken);

		/// <inheritdoc />
		public async Task<VideoComment> GetCommentByIdAsync(string id, CancellationToken cancellationToken) =>
			await CommentDbSet.FindAsync(new object[] { id }, cancellationToken);

		public Task<PaginatedResult<VideoComment>> ListCommentsPaginatedByFilterAsync(string videoId, PagingParameters pagingParameters, CancellationToken cancellationToken)
		{
			var query = CommentDbSet.Where(it => it.Video.Id == videoId);

			return base.ListPaginatedAsync(query, pagingParameters, cancellationToken);
		}

		public async Task CreateCommentAsync(VideoComment comment, CancellationToken cancellationToken)
		{
			await CommentDbSet.AddAsync(comment, cancellationToken);
		}

		public async Task DeleteCommentAsync(VideoComment comment, CancellationToken cancellationToken) =>
			await CommentDbSet
				.Where(it => it.Id == comment.Id)
				.ExecuteDeleteAsync(cancellationToken);
	}
}
