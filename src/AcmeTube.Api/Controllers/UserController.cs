using AcmeTube.Api.Services;
using AcmeTube.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcmeTube.Api.Controllers;

[Authorize]
[Route("users")]
public sealed class UserController : ApiController
{
	private readonly UserAppService _service;

	public UserController(UserAppService service, IOperationContextManager operationContextManager)
		: base(operationContextManager)
	{
		_service = service;
	}



}