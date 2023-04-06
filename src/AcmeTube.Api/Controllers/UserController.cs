using AcmeTube.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using AcmeTube.Domain.Security;
using AcmeTube.Infrastructure.Security;

namespace AcmeTube.Api.Controllers;

[Authorize]
[Route("users")]
public sealed class UserController : ApiController
{
	private readonly UserAppService _service;

	public UserController(UserAppService service) => _service = service;

	[HasPermission(PermissionType.UserLock)]
	[HttpPost("{id}/lock")]
	public async Task<IActionResult> LockAsync(string id, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.LockAccountAsync(id, cancellationToken).ConfigureAwait(false));

	[HasPermission(PermissionType.UserUnlock)]
	[HttpPost("{id}/unlock")]
	public async Task<IActionResult> UnlockAsync(string id, CancellationToken cancellationToken) =>
		BuildActionResult(await _service.UnlockAccountAsync(id, cancellationToken).ConfigureAwait(false));
}