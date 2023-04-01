using AcmeTube.Application.Services;
using AcmeTube.Domain.Commons;
using MediatR;

namespace AcmeTube.Application.Core.Commands
{
    public interface ICommand<out TCommandResult> : IRequest<TCommandResult>
        where TCommandResult : CommandResult { }

    public interface ICommand : ICommand<CommandResult> { }

    public record Command<TCommandResult>(bool BypassValidation = false) : ICommand<TCommandResult>, IOperationContextSetup
		where TCommandResult : CommandResult
    {
        public string GetMembershipId() => Context?.Identity?.Id;

        public OperationContext Context { get; private set; }
        
	    void IOperationContextSetup.Setup(OperationContext context) => Context = context;
    }
}
