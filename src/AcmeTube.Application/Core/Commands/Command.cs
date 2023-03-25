using AcmeTube.Domain.Commons;
using MediatR;

namespace AcmeTube.Application.Core.Commands
{
    public interface ICommand<out TCommandResult> : IRequest<TCommandResult>
        where TCommandResult : CommandResult { }

    public interface ICommand : ICommand<CommandResult> { }

    public record Command<TCommandResult>(OperationContext OperationContext, bool BypassValidation = false) : ICommand<TCommandResult>
        where TCommandResult : CommandResult;
}
