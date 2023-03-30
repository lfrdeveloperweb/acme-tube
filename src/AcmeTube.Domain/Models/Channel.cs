using System.Collections.Generic;

namespace AcmeTube.Domain.Models
{
    public sealed class Channel : EntityBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }
		public ICollection<string> Tags { get; set; }
        public IDictionary<string, string> Links { get; set; }
		public ChannelStats Stats { get; set; }
	}

    public sealed class ChannelStats
    {
	    public int VideosCount { get; set; }
        public int ViewsCount { get; set; }
        public int SubscribersCount { get; set; }
    }
}
