using AcmeTube.Api.Constants;
using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using Microsoft.AspNetCore.Http;

namespace AcmeTube.Api.Services
{
    /// <summary>
    /// Request manager.
    /// </summary>
    public sealed class OperationContextManager : IOperationContextManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public OperationContextManager(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }

        /// <summary>
        /// Retrieve context of a request.
        /// </summary>
        public OperationContext GetContext()
        {
            var context = _httpContextAccessor.HttpContext;
            var connection = context.Request.HttpContext.Connection;

            return new OperationContext(
                CorrelationId: context.Request.Headers[ApplicationConstants.HeaderNames.RequestId],
                Identity: _identityService.GetIdentity(),
                InternalSourceIp: connection.LocalIpAddress?.ToString(),
                ExternalSourceIp: connection.RemoteIpAddress?.ToString());
        }
    }
}
