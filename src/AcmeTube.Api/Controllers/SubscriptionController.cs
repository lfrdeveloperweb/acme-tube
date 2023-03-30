using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Api.Extensions;
using AcmeTube.Api.Services;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
		BuildActionResult(await _service.ListChannelSubscriptionsAsync(channelId, pagingParameters, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));


	//[HttpGet("~/users/{userId}/subscriptions")]
	//public async Task<IActionResult> ListUserSubscriptions(string userId, [FromQuery] PagingParameters pagingParameters, CancellationToken cancellationToken) =>
	//	BuildActionResult(await _service.ListUserSubscriptionsAsync(userId, pagingParameters, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

	[HttpPost("{id}/subscriptions")]
	public async Task<IActionResult> Subscribe(string id, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.SubscribeAsync(id, base.OperationContextManager.GetContext(), cancellationToken));

	[HttpDelete("{id}/subscriptions")]
	public async Task<IActionResult> Unsubscribe(string id, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.UnsubscribeAsync(id, base.OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));
}