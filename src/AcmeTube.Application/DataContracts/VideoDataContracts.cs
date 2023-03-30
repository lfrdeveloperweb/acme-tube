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
            ICollection<string> Tags);

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
			string CreatedBy,
			DateTimeOffset CreatedAt,
			string UpdatedBy,
			DateTimeOffset? UpdatedAt);

        public sealed record VideoCommentResponseData(
            string Id,
            string Description,
            DateTimeOffset CreatedAt);
    }
}
