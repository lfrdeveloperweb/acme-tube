using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Api.Controllers;

[Authorize]
[Route("subscriptions")]
public sealed class SubscriptionController : ApiController
{
	private readonly SubscriptionAppService _service;

	public SubscriptionController(SubscriptionAppService service, IOperationContextManager operationContextManager)
		: base(operationContextManager) => _service = service;

	/// <summary>
	/// Get task by id.
	/// </summary>
	[AllowAnonymous]
	[HttpGet("~/channels/{channelId}/subscriptions")]
	public async Task<IActionResult> ListChannelsSubscriptions(string channelId, [FromQuery] PagingParameters pagingParameters, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.ListChannelSubscriptionsAsync(channelId, pagingParameters, cancellationToken).ConfigureAwait(false));


	[HttpGet("~/accounts/subscriptions")]
	public async Task<IActionResult> ListUserSubscriptions([FromQuery] PagingParameters pagingParameters, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.ListUserSubscriptionsAsync(pagingParameters, cancellationToken).ConfigureAwait(false));

	[HttpPost]
	public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.SubscribeAsync(request?.ChannelId, cancellationToken));

	[HttpDelete]
	public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeRequest request, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.UnsubscribeAsync(request?.ChannelId, cancellationToken).ConfigureAwait(false));
}