using System.IO;
using AcmeTube.Api.Constants;
using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Security;
using AcmeTube.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace AcmeTube.Api.Controllers
{
    [Authorize]
    [Route("videos")]
    public sealed class VideoController : ApiController
    {
        private readonly VideoAppService _service;
        private readonly IMediaService _mediaService;

        public VideoController(VideoAppService service, IMediaService mediaService)
        {
	        _service = service;
	        _mediaService = mediaService;
        }

        /// <summary>
        /// Get task by id.
        /// </summary>
        [HasPermission(PermissionType.VideoFull)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .GetAsync(id, cancellationToken)
                .ConfigureAwait(false));

        /// <summary>
        /// Search task by filter.
        /// </summary>
        //[ResourceAuthorization(PermissionType.VideoRead)]
        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<IActionResult> Search(PagingParameters pagingParameters, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .SearchAsync(pagingParameters, cancellationToken)
                .ConfigureAwait(false));
       
        [Consumes(ApplicationConstants.ContentTypes.FormData)]
		[HttpPost]
        public async Task<IActionResult> Create([FromForm] VideoForCreationRequest request, IFormFile file, CancellationToken cancellationToken)
        {
	        if (request == null || file == null || file.Length == 0) 
		        return BadRequest();

	        var fileRequest = await GetFileAsync(file);

	        var result = await _mediaService.GetVideoMetadata(fileRequest.Content, Path.GetExtension(file.FileName));



			

			return BuildActionResult(await _service
		        .CreateAsync(request, fileRequest, cancellationToken)
		        .ConfigureAwait(false));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] VideoForUpdateRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service.UpdateAsync(id, request, cancellationToken).ConfigureAwait(false));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service.DeleteAsync(id, cancellationToken).ConfigureAwait(false));

		[HttpPost("{id}/rate/like")]
		[HttpPost("{id}/rate/dislike")]
		public async Task<IActionResult> PostRatingVideo(string id, CancellationToken cancellationToken) =>
			BuildActionResult(await _service.CreateRatingAsync(id, Request!.Path!.Value!.EndsWith("/like", StringComparison.OrdinalIgnoreCase), cancellationToken).ConfigureAwait(false));

		[HttpDelete("{id}/rate")]
		public async Task<IActionResult> DeleteRatingVideo(string id, CancellationToken cancellationToken) =>
			BuildActionResult(await _service.DeleteRatingAsync(id, cancellationToken).ConfigureAwait(false));

		/// <summary>
		/// Search comments by filter.
		/// </summary>
		[HttpPost("{videoId}/comments/search")]
        public async Task<IActionResult> SearchComments(string videoId, PagingParameters pagingParameters, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .SearchCommentsAsync(videoId, pagingParameters, cancellationToken)
                .ConfigureAwait(false));

        [HttpPost("{videoId}/comments")]
        public async Task<IActionResult> PostComment(string videoId, [FromBody] VideoCommentForCreationRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service.CreateCommentAsync(videoId, request, cancellationToken).ConfigureAwait(false));

        [HttpDelete("{videoId}/comments/{id}")]
        public async Task<IActionResult> DeleteComment(string id, string videoId, CancellationToken cancellationToken) =>
            BuildActionResult(await _service.DeleteCommentAsync(id, videoId, cancellationToken).ConfigureAwait(false));
    }
}
