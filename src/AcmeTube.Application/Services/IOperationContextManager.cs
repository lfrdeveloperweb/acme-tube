using AcmeTube.Domain.Commons;

namespace AcmeTube.Application.Services
{
	public interface IOperationContextManager
	{
		/// <summary>
		/// Retrieve context of a request.
		/// </summary>
		OperationContext GetContext();
	}
}