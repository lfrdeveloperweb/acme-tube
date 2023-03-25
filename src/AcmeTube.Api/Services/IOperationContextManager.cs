using AcmeTube.Domain.Commons;

namespace AcmeTube.Api.Services;

public interface IOperationContextManager
{
    /// <summary>
    /// Retrieve context of a request.
    /// </summary>
    OperationContext GetContext();
}