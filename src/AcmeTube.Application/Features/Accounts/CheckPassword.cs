using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Security;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Resources;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Accounts
{
    public static class CheckPassword
    {
        public sealed record Command(
            string Password,
            OperationContext Context) : Command<CommandResult>(Context);

        internal sealed class CommandHandler : CommandHandler<Command>
        {
            private readonly ISecurityService _securityService;

            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                ISecurityService securityService) : base(loggerFactory, unitOfWork)
            {
                _securityService = securityService;
            }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.UserRepository.GetByIdAsync(command.Context.Identity.Id, cancellationToken);

                var checkPasswordResult = await _securityService.CheckPasswordAsync(user, command.Password);

                return checkPasswordResult.Succeeded
                    ? CommandResult.NoContent()
                    : CommandResult.UnprocessableEntity<CommandResult>(checkPasswordResult.IsLockedOut
                        ? ReportCodeType.UserIsLockedOut
                        : ReportCodeType.InvalidPassword);
            }
        }

        internal sealed class CommandValidator : CommandValidator<CheckPassword.Command>
        {
            public CommandValidator()
            {
                RuleFor(command => command.Password)
                    .NotNullOrEmpty()
                    .Password();
            }
        }
    }
}
