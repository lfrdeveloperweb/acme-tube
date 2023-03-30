using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Commons;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AcmeTube.Application.Features.Subscriptions;

public static class UnsubscribeChannel
{
	public sealed record Command(
		string ChannelId,
		OperationContext Context) : Command<CommandResult>(Context);

	public sealed class CommandHandler : CommandHandler<Command>
	{
		public CommandHandler(
			ILoggerFactory loggerFactory,
			IUnitOfWork unitOfWork) : base(loggerFactory, unitOfWork) { }

		protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
		{
			if (await UnitOfWork.SubscriptionRepository.GetAsync(command.ChannelId, command.Context.Identity.Id, cancellationToken) is not { } subscription)
			{
				return CommandResult.NotFound();
			}
			
			await UnitOfWork.SubscriptionRepository.UnsubscribeAsync(subscription, cancellationToken);

			return CommandResult.NoContent();
		}
	}
}