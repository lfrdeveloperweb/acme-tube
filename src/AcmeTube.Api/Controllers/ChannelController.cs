using AcmeTube.Api.Extensions;
using AcmeTube.Api.Services;
using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Api.Controllers;

public class SampleActionFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = await next();
	}
}

[Authorize]
[Route("channels")]
public sealed class ChannelController : ApiController
{
    private readonly ChannelAppService _service;

    public ChannelController(ChannelAppService service, IOperationContextManager operationContextManager)
        : base(operationContextManager) => _service = service;

	/// <summary>
	/// Get task by id.
	/// </summary>
	[AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) =>
        (await _service.GetAsync(id, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false)).BuildActionResult();

    /// <summary>
    /// Search task by filter.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("search")]
    public async Task<IActionResult> Search(PagingParameters pagingParameters, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.SearchAsync(pagingParameters, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

	//[ResourceAuthorization(PermissionType.ChannelFull, PermissionType.ChannelCreate)]
	[HttpPost]
    public async Task<IActionResult> Post([FromBody] ChannelForCreationRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.CreateAsync(request, base.OperationContextManager.GetContext(), cancellationToken));

	//[ResourceAuthorization(PermissionType.ChannelFull, PermissionType.ChannelDelete)]
	[HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.DeleteAsync(id, base.OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));
}