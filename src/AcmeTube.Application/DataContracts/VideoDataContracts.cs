using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmeTube.Application.DataContracts
{
    namespace Requests
    {
        public sealed record VideoForCreationRequest(
            [Required] string Title,
            string Description,
            string ChannelId,
            ICollection<string> Tags,
            bool? IsPublic);

        public sealed record VideoForUpdateRequest(
            [Required] string Title,
            string Description,
            string ChannelId,
            ICollection<string> Tags);

        public sealed record VideoCommentForCreationRequest(
            string Description);
    }

    namespace Responses
    {
        public sealed record VideoResponseData(
            string Id,
            string Title,
            string Description,
            ICollection<string> Tags,
            bool IsPublic,
            string VideoFilePath,
            string VideoUrl,
            string VideoExternalId,
            string ThumbnailFilePath,
            string ThumbnailUrl,
            string ThumbnailExternalId,
			VideoStatsResponseData Stats,
			string CreatedBy,
			DateTimeOffset CreatedAt,
			string UpdatedBy,
			DateTimeOffset? UpdatedAt);

        public sealed record VideoStatsResponseData(
	        int ViewsCount,
	        int LikesCount,
	        int DislikesCount,
	        int CommentsCount);

		public sealed record VideoCommentResponseData(
            string Id,
            string Description,
            DateTimeOffset CreatedAt);
    }
}
