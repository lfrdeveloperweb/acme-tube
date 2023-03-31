using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Accounts
{
	public static class UnlockAccount
	{
		public sealed record Command(
			string UserId) : Command<CommandResult>;

		internal sealed class CommandHandler : CommandHandler<Command>
		{
			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork) : base(loggerFactory, unitOfWork) { }

			protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var user = await UnitOfWork.UserRepository.GetByIdAsync(command.UserId, cancellationToken);
				if (user is null) return CommandResult.NotFound();

				if (!user.IsLocked) return CommandResult.NoContent();

				user.Unlock();

				await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

				return CommandResult.NoContent();
			}
		}
	}
}