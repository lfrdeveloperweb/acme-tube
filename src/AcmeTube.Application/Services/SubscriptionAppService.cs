using AcmeTube.Application.DataContracts.Responses;
using AcmeTube.Application.Features.Subscriptions;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Models;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;

namespace AcmeTube.Application.Services;

public sealed class SubscriptionAppService : AppServiceBase
{
	public SubscriptionAppService(ISender sender, IMapper mapper) : base(sender, mapper) { }
	
	public async ValueTask<PaginatedResponse<SubscriptionUserResponseData>> ListChannelSubscriptionsAsync(string channelId, PagingParameters pagingParameters, OperationContext context, CancellationToken cancellationToken)
	{
		var query = new ListChannelUserSubscribersPaginated.Query(channelId, pagingParameters, context);
		var result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

		return Response.From<User, SubscriptionUserResponseData>(result, Mapper);
	}

	public async ValueTask<PaginatedResponse<SubscriptionChannelResponseData>> ListUserSubscriptionsAsync(PagingParameters pagingParameters, OperationContext context, CancellationToken cancellationToken)
	{
		var query = new ListUserChannelSubscribersPaginated.Query(pagingParameters, context);
		var result = await Sender.Send(query, cancellationToken).ConfigureAwait(false);

		return Response.From<Channel, SubscriptionChannelResponseData>(result, Mapper);
	}

	public async ValueTask<Response> SubscribeAsync(string channelId, OperationContext operationContext, CancellationToken cancellationToken) => 
		Response.From(await Sender.Send(new SubscribeChannel.Command(channelId, operationContext), cancellationToken));

	public async ValueTask<Response> UnsubscribeAsync(string channelId, OperationContext operationContext, CancellationToken cancellationToken) => 
		Response.From(await Sender.Send(new UnsubscribeChannel.Command(channelId, operationContext), cancellationToken));
}