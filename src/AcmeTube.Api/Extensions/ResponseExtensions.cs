using AcmeTube.Application.DataContracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcmeTube.Api.Extensions
{
	public static class ResponseExtensions
	{
		public static IActionResult BuildActionResult<TResponse>(this TResponse response) where TResponse : Response =>
			response.StatusCode switch
			{
				StatusCodes.Status200OK => new OkObjectResult(response),
				//StatusCodes.Status201Created => StatusCode(response.StatusCode, response),
				StatusCodes.Status202Accepted => new AcceptedResult(),
				StatusCodes.Status204NoContent => new NoContentResult(),
				//StatusCodes.Status206PartialContent => StatusCode(response.StatusCode, response),
				StatusCodes.Status401Unauthorized => new UnauthorizedResult(),
				StatusCodes.Status403Forbidden => new ForbidResult(),
				StatusCodes.Status404NotFound => new NotFoundResult(),
				//StatusCodes.Status422UnprocessableEntity => StatusCode(response.StatusCode, response),
				//StatusCodes.Status503ServiceUnavailable => StatusCode(response.StatusCode, response),
				_ => new ObjectResult(response) { StatusCode = response.StatusCode }
			};
	}
}
