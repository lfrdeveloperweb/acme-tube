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
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Labels);

        public sealed record VideoForUpdateRequest(
            [Required] string Title,
            string Description,
            string ChannelId,
            DateTime? DueDate,
            [Required] int Priority,
            ICollection<string> Labels);

        public sealed record VideoCommentForCreationRequest(
            string Description);
    }

    namespace Responses
    {
        public sealed record VideoResponseData(
            string Id,
            string Title,
            string Description,
            DateTime? DueDate,
            int Priority,
            ICollection<string> Labels,
            DateTimeOffset? CompletedAt,
            DateTimeOffset CreatedAt,
            IdentityNamedResponse CreatedBy,
            DateTimeOffset? UpdatedAt);

        public sealed record VideoCommentResponseData(
            string Id,
            string Description,
            DateTimeOffset CreatedAt);
    }
}
