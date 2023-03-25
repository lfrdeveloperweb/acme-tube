using System;
using System.ComponentModel.DataAnnotations;

namespace AcmeTube.Application.DataContracts
{
    namespace Requests
    {
        public sealed record ChannelForCreationRequest(
            [Required] string Title,
            string Color);
    }

    namespace Responses
    {
        public sealed record ChanneltResponseData(
            string Title,
            string Color,
            DateTimeOffset CreatedAt);
    }
}
