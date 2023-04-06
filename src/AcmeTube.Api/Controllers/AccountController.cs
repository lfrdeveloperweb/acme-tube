﻿using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Security;
using AcmeTube.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Api.Controllers;

[Route("accounts")]
public sealed class AccountController : ApiController
{
    private readonly AccountAppService _service;

    public AccountController(AccountAppService service) => _service = service;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAccountAsync([FromBody] RegisterAccountRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.RegisterAccountAsync(request, cancellationToken).ConfigureAwait(false));
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.LoginAsync(request, cancellationToken).ConfigureAwait(false));

    [Authorize]
	[HasPermission(PermissionType.UserRead)]
	[HttpGet("profile")]
    public async Task<IActionResult> ProfileAsync(CancellationToken cancellationToken) =>
        BuildActionResult(await _service.GetProfileAsync(cancellationToken).ConfigureAwait(false));

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ConfirmEmailAsync(request, cancellationToken).ConfigureAwait(false));

    [HttpPost("confirm-phone-number")]
    public async Task<IActionResult> ConfirmPhoneNumber([FromBody] ConfirmPhoneNumberRequest request, CancellationToken cancellationToken) => Ok();

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ForgotPasswordAsync(request, cancellationToken).ConfigureAwait(false));

    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ResetPasswordAsync(request, cancellationToken).ConfigureAwait(false));

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ChangePasswordAsync(request, cancellationToken).ConfigureAwait(false));
}