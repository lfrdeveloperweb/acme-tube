﻿using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AcmeTube.Domain.Models.Filters;

namespace AcmeTube.Application.Repositories
{
    public interface IVideoRepository
    {
        /// <summary>
        /// Retrieves an <see cref="Video"/> by its identifier.
        /// </summary>
        Task<Video> GetByIdAsync(string id, CancellationToken cancellationToken);

        Task<PaginatedResult<Video>> ListPaginatedByFilterAsync(VideoFilter filter, PagingParameters pagingParameters, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string id, CancellationToken cancellationToken);

        Task CreateAsync(Video video, CancellationToken cancellationToken);

        Task UpdateAsync(Video video, CancellationToken cancellationToken);

        Task DeleteAsync(Video video, CancellationToken cancellationToken);

        Task<VideoRatingType?> GetRatingVideoAsync(string videoId, string membershipId, CancellationToken cancellationToken);

        Task UpsertRatingVideoAsync(string videoId, string membershipId, VideoRatingType ratingType, CancellationToken cancellationToken);

        Task<bool> IncreaseVideoViewsCountAsync(string videoId, string membershipId, CancellationToken cancellationToken);

        Task<bool> DeleteRatingVideoAsync(string videoId, string membershipId, CancellationToken cancellationToken);

		Task<PaginatedResult<VideoComment>> ListCommentsPaginatedByFilterAsync(string videoId,
	        PagingParameters pagingParameters, CancellationToken cancellationToken);

        Task<VideoComment> GetCommentByIdAsync(string id, CancellationToken cancellationToken);

        Task CreateCommentAsync(VideoComment comment, CancellationToken cancellationToken);

        Task<bool> DeleteCommentAsync(int comment, CancellationToken cancellationToken);
    }
}
