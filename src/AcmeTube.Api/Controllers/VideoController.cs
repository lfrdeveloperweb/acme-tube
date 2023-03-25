using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Api.Services;
using AcmeTube.Application.DataContracts.Requests;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Security;
using AcmeTube.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcmeTube.Api.Controllers
{
    [Authorize]
    [Route("videos")]
    public sealed class VideoController : ApiController
    {
        private readonly VideoAppService _service;

        public VideoController(VideoAppService service, IOperationContextManager operationContextManager)
            : base(operationContextManager)
        {
            _service = service;
        }

        /// <summary>
        /// Get task by id.
        /// </summary>
        [HasPermission(PermissionType.VideoRead)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .GetAsync(id, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        /// <summary>
        /// Search task by filter.
        /// </summary>
        //[ResourceAuthorization(PermissionType.VideoRead)]
        [AllowAnonymous]
        [HttpPost("search")]
        public async Task<IActionResult> Search(PagingParameters pagingParameters, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .SearchAsync(pagingParameters, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VideoForCreationRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .CreateAsync(request, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPost("{id}/clone")]
        public async Task<IActionResult> Clone(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .CloneAsync(id, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] VideoForUpdateRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .UpdateAsync(id, request, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .DeleteAsync(id, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));


        /// <summary>
        /// Search comments by filter.
        /// </summary>
        [HttpPost("{videoId}/comments/search")]
        public async Task<IActionResult> SearchComments(string videoId, PagingParameters pagingParameters, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .SearchCommentsAsync(videoId, pagingParameters, OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpPost("{videoId}/comments")]
        public async Task<IActionResult> PostComment(string videoId, [FromBody] VideoCommentForCreationRequest request, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .CreateCommentAsync(videoId, request, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));

        [HttpDelete("{videoId}/comments/{id}")]
        public async Task<IActionResult> DeleteComment(string id, string videoId, CancellationToken cancellationToken) =>
            BuildActionResult(await _service
                .DeleteCommentAsync(id, videoId, base.OperationContextManager.GetContext(), cancellationToken)
                .ConfigureAwait(false));
    }
}
