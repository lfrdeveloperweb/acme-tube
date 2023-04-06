namespace AcmeTube.Application.Settings
{
	public sealed record MediaSettings
	{
		public string ChannelVideoPathTemplate { get; init; }
		public string[] SupportedContentTypes { get; init; }
		public int MaximumSizeInMegabytes { get; init; }
	}
}
