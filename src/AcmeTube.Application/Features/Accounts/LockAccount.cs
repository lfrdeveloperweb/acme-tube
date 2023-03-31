using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Accounts
{
	public static class LockAccount
	{
		public sealed record Command(string UserId) : Command<CommandResult>;

		internal sealed class CommandHandler : CommandHandler<Command>
		{
			private readonly ISystemClock _systemClock;

			public CommandHandler(
				ILoggerFactory loggerFactory,
				IUnitOfWork unitOfWork,
				ISystemClock systemClock) : base(loggerFactory, unitOfWork)
			{
				_systemClock = systemClock;
			}

			protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
			{
				var user = await UnitOfWork.UserRepository.GetByIdAsync(command.UserId, cancellationToken);
				if (user is null) return CommandResult.NotFound();

				if (user.IsLocked) return CommandResult.NoContent();

				user.Lock(_systemClock.UtcNow);

				await UnitOfWork.UserRepository.UpdateAsync(user, cancellationToken);

				return CommandResult.NoContent();
			}
		}
	}
}