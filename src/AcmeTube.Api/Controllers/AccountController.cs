﻿using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Api.Services;
using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcmeTube.Api.Controllers;

[Route("accounts")]
public sealed class AccountController : ApiController
{
    private readonly AccountAppService _service;

    public AccountController(AccountAppService service, IOperationContextManager operationContextManager)
        : base(operationContextManager)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAccountAsync([FromBody] RegisterAccountRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.RegisterAccountAsync(request, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.LoginAsync(request, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> ProfileAsync(CancellationToken cancellationToken) =>
        BuildActionResult(await _service.GetProfileAsync(OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ConfirmEmailAsync(request, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPost("confirm-phone-number")]
    public async Task<IActionResult> ConfirmPhoneNumber([FromBody] ConfirmPhoneNumberRequest request, CancellationToken cancellationToken) => Ok();

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ForgotPasswordAsync(request, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPut("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ResetPasswordAsync(request, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.ChangePasswordAsync(request, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPost("{id}/lock")]
    public async Task<IActionResult> LockAsync(string id, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.LockAccountAsync(id, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

    [HttpPost("{id}/unlock")]
    public async Task<IActionResult> UnlockAsync(string id, CancellationToken cancellationToken) =>
        BuildActionResult(await _service.UnlockAccountAsync(id, OperationContextManager.GetContext(), cancellationToken).ConfigureAwait(false));

}