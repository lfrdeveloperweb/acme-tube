using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Core.Cryptography;
using AcmeTube.Application.Extensions;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using AcmeTube.Domain.Resources;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Accounts
{
	public static class ResetPassword
	{
		public record Command(
			string Email,
			string CurrentPassword,
			string NewPassword,
			string ConfirmNewPassword,
			string Token) : Command<CommandResult>;

		public sealed class CommandHandler : CommandHandler<Command>
		{
			private readonly IPasswordHasher _passwordHasher;

			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork,
				ICommandValidator<Command> validator,
				IPasswordHasher passwordHasher) : base(loggerFactory, unitOfWork, validator)
			{
				_passwordHasher = passwordHasher;
			}

			protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var user = await UnitOfWork.UserRepository.GetByEmailAsync(command.Email, cancellationToken);
				if (user is null) return CommandResult.NotFound();

				if (!_passwordHasher.VerifyHashedPassword(user.PasswordHash, command.CurrentPassword))
				{
					return CommandResult.UnprocessableEntity(Report.Create(ReportCodeType.PasswordMismatch));
				}

				//var confirmPasswordResetResult = await _userManager.ResetPasswordAsync(user, command.Token, command.NewPassword);
				//if (!confirmPasswordResetResult.Succeeded)
				//{
				//    return CommandResult.UnprocessableEntity<CommandResult>(ErrorsFrom(confirmPasswordResetResult));
				//}

				Logger.LogWarning("Reset password applied to user {UserId} with email {Email}.", user.Id, user.Email);

				return CommandResult.NoContent();
			}

			public class CommandValidator : CommandValidator<Command>
			{
				public CommandValidator()
				{
					RuleFor(command => command.Email)
						.NotNullOrEmpty()
						.IsValidEmail();

					RuleFor(command => command.Token)
						.NotNullOrEmpty();

					RuleFor(command => command.CurrentPassword)
						.NotNullOrEmpty();

					RuleFor(request => request.NewPassword)
						.NotNullOrEmpty()
						.Password();

					RuleFor(request => request.ConfirmNewPassword)
						.NotNullOrEmpty()
						.Equal(request => request.NewPassword)
						.WithMessageFromErrorCode(ReportCodeType.ConfirmPasswordNotMatch);
				}
			}
		}
	}
}