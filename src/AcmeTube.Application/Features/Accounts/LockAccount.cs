using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Accounts
{
	public static class LockAccount
	{
		public sealed record Command(
			string UserId,
			OperationContext Context) : Command<CommandResult>(Context);

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