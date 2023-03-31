using AcmeTube.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.PipelineBehaviors
{
	public sealed class OperationContextPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>, IOperationContextSetup
	{
		private readonly IOperationContextManager _operationContextManager;

		public OperationContextPipelineBehavior(IOperationContextManager operationContextManager)
		{
			_operationContextManager = operationContextManager;
		}

		public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			request.Setup(_operationContextManager.GetContext());

			return next();
		}
	}
}
