using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcmeTube.Application.DataContracts
{
    namespace Requests
    {
	    public sealed record ChannelForCreationRequest(
		    [Required] string Name,
		    string Description,
		    string CountryName,
		    ICollection<string> Tags,
		    IDictionary<string, string> Links);
	}

    namespace Responses
    {
        public sealed record ChannelResponseData(
            string Id,
            string Name,
            string Description,
            string CountryName,
            ICollection<string> Tags,
            ChannelStatsResponseData Stats,
            IDictionary<string, string> Links,
            string CreatedBy,
			DateTimeOffset CreatedAt,
            string UpdatedBy,
            DateTimeOffset? UpdatedAt);

        public sealed record ChannelStatsResponseData(
			int VideosCount,
            int ViewsCount,
            int SubscribersCount);
	}
}
