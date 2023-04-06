using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Security;
using AcmeTube.Infrastructure.Security;

namespace AcmeTube.Api.Controllers;

[Authorize]
[Route("channels")]
public sealed class ChannelController : ApiController
{
    private readonly ChannelAppService _service;

    public ChannelController(ChannelAppService service) => _service = service;

	/// <summary>
	/// Get task by id.
	/// </summary>
	[AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.GetAsync(id, cancellationToken).ConfigureAwait(false));

    /// <summary>
    /// Search task by filter.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("search")]
    public async Task<IActionResult> Search(PagingParameters pagingParameters, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.SearchAsync(pagingParameters, cancellationToken).ConfigureAwait(false));

	[HasPermission(PermissionType.ChannelCreate)]
	[HttpPost]
    public async Task<IActionResult> Post([FromBody] ChannelForCreationRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.CreateAsync(request, cancellationToken));

	[HasPermission(PermissionType.ChannelDelete)]
	[HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));
}