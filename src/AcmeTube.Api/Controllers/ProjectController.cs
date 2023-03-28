using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Api.Extensions;
using AcmeTube.Api.Services;
using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AcmeTube.Api.Controllers;

public class SampleActionFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var result = await next();
	}
}

[Route("channels")]
public sealed class ChannelController : ApiController
{
    private readonly ChannelAppService _service;

    public ChannelController(ChannelAppService service, IOperationContextManager operationContextManager)
        : base(operationContextManager) => _service = service;

    /// <summary>
    /// Get task by id.
    /// </summary>
    //[Permission(PermissionType.OrderRead)]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) =>
        (await _service.GetAsync(id, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false)).BuildActionResult();

    /// <summary>
    /// Search task by filter.
    /// </summary>
    //[ResourceAuthorization(PermissionType.OrderFull, PermissionType.OrderRead)]
    [HttpPost("search")]
    public async Task<IActionResult> Search(PagingParameters pagingParameters, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.SearchAsync(pagingParameters, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChannelForCreationRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.CreateAsync(request, base.OperationContextManager.GetContext(), cancellationToken));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.DeleteAsync(id, base.OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));
}