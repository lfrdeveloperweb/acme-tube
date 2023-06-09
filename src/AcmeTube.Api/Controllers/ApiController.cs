﻿using AcmeTube.Application.DataContracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using AcmeTube.Domain.Commons;

namespace AcmeTube.Api.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        [NonAction]
        protected IActionResult BuildActionResult<TResponse>(TResponse response) where TResponse : Response =>
            response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(response),
                StatusCodes.Status201Created => StatusCode(response.StatusCode, response),
                StatusCodes.Status202Accepted => Accepted(),
                StatusCodes.Status204NoContent => NoContent(),
                StatusCodes.Status206PartialContent => StatusCode(response.StatusCode, response),
                StatusCodes.Status401Unauthorized => Unauthorized(response),
                StatusCodes.Status403Forbidden => Forbid(),
                StatusCodes.Status404NotFound => NotFound(),
                StatusCodes.Status422UnprocessableEntity => StatusCode(response.StatusCode, response),
                StatusCodes.Status503ServiceUnavailable => StatusCode(response.StatusCode, response),
                _ => StatusCode(response.StatusCode, response)
            };

        /// <summary>
        /// Read all bytes from <see cref="IFormFile"/>.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected static async Task<FileMetadata> GetFileAsync(IFormFile file)
        {
	        using var stream = new MemoryStream();
	        
	        await file.CopyToAsync(stream);

	        return new (file.FileName, file.ContentType, file.Length, stream.ToArray());
        }
	}
}
