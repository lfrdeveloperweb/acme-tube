namespace AcmeTube.Application.DataContracts
{
	namespace Requests
	{
		public sealed record SubscribeRequest(string ChannelId);

	    public sealed record UnsubscribeRequest(string ChannelId);
	}

    namespace Responses
	{
		public sealed record SubscriptionChannelResponseData(
	        string Id,
	        string Name);

		public sealed record SubscriptionUserResponseData(
	        string Id,
	        string Name);
	}
}
