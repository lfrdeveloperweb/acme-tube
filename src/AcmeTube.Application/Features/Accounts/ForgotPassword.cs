using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Commons;
using AcmeTube.Application.Core.Security;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Application.Settings;
using AcmeTube.Domain.Resources;
using AcmeTube.Domain.Security;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Events;

namespace AcmeTube.Application.Features.Accounts
{
    public static class ForgotPassword
    {
        public sealed record Command(string DocumentNumber) : Command<CommandResult>;

        internal sealed class CommandHandler : CommandHandler<Command>
        {
            private readonly IPublisher _bus;
            private readonly ISecurityService _securityService;

            public CommandHandler(
                ILoggerFactory loggerFactory,
                IUnitOfWork unitOfWork,
                IPublisher bus,
                ISecurityService securityService) : base(loggerFactory, unitOfWork)
            {
	            _bus = bus;
	            _securityService = securityService;
            }

            protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
            {
                if (await UnitOfWork.UserRepository.GetByDocumentNumberAsync(command.DocumentNumber, cancellationToken) is not { } user) 
					return CommandResult.NotFound();

                var token = await _securityService.GeneratePasswordResetTokenAsync(user, cancellationToken);

                await _bus.Publish(new ForgotPasswordEvent(user.DocumentNumber, user.Email, token), cancellationToken);

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
