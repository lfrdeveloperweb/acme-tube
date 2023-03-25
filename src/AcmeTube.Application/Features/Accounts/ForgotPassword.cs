using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.Core.Security;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Application.Settings;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Events.Accounts;
using AcmeTube.Domain.Resources;
using AcmeTube.Domain.Security;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AcmeTube.Application.Features.Accounts
{
    public static class ForgotPassword
    {
        public record Command(string DocumentNumber, OperationContext Context) : Command<CommandResult>(Context);

        internal sealed class CommandHandler : CommandHandler<Command>
        {
            private readonly IPublisher _bus;
            private readonly ISecurityService _securityService;
            private readonly IKeyGenerator _keyGenerator;
            private readonly ISystemClock _systemClock;
            private readonly AccountSettings _accountSettings;

            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                IPublisher bus,
                ISecurityService securityService,
                IKeyGenerator keyGenerator,
                ISystemClock systemClock,
                IOptionsSnapshot<AccountSettings> accountSettings) : base(loggerFactory, unitOfWork)
            {
                _bus = bus;
                _securityService = securityService;
                _keyGenerator = keyGenerator;
                _systemClock = systemClock;
                _accountSettings = accountSettings.Value;
            }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                var user = await UnitOfWork.UserRepository.GetByDocumentNumberAsync(command.DocumentNumber, cancellationToken);
                if (user is null) return CommandResult.NotFound();

                var userToken = new UserToken<UserResetPasswordTokenData>(
                    userId: user.Id,
                    value: _keyGenerator.Generate(),
                    expiresAt: _systemClock.UtcNow.AddMinutes(_accountSettings.PasswordResetTokenExpirationInMinutes),
                    type: UserTokenType.ResetPasswordToken,
                    data: user.Email);

                await UnitOfWork.UserRepository.CreateUserTokenAsync(userToken);

                await _bus.Publish(new ForgotPasswordEvent(command.DocumentNumber, userToken.Value), cancellationToken);

                return CommandResult.NoContent();
            }
        }

        internal sealed class CommandValidator : CommandValidator<Command>
        {
            public CommandValidator(IUnitOfWork unitOfWork)
            {
                RuleFor(command => command.DocumentNumber)
                    .IsValidEmail()
                    .MustAsync(unitOfWork.UserRepository.ExistByEmailAsync)
                    .WithMessageFromErrorCode(ReportCodeType.InvalidEmail);
            }
        }
    }
}
