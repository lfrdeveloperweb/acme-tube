using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Security;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Accounts
{
    public static class ConfirmEmail
    {
        public record Command(
            string Email,
            string Token) : Command<CommandResult>;

        internal sealed class CommandHandler : CommandHandler<Command>
        {
            private readonly ISystemClock _systemClock;

            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                ICommandValidator<Command> validator,
                ISystemClock systemClock) : base(loggerFactory, unitOfWork, validator)
            {
                _systemClock = systemClock;
            }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                if (await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken) is not { } user) 
                    return CommandResult.NotFound();

                if (await UnitOfWork.UserRepository.GetAsync<UserEmailConfirmationTokenData>(user.Id, UserTokenType.EmailConfirmationToken, command.Token, cancellationToken) is not { } userToken)
	                return CommandResult.NotFound();
                
                user.ConfirmEmail();
                user.UpdatedBy = user.Id;

                await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);
                await UnitOfWork.UserRepository.DeleteTokenAsync(userToken.UserId, userToken.Type);

                return CommandResult.NoContent();
            }
        }

        internal sealed class CommandValidator : CommandValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(command => command.Email)
                    .NotNullOrEmpty();

                RuleFor(command => command.Token)
                    .NotNullOrEmpty();
            }
        }
    }
}
