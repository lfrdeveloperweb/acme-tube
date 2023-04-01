using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Events;
using AcmeTube.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AcmeTube.Application.Features.Videos;

public static class DeleteRatingVideo
{
	public record Command(string VideoId) : Command<CommandResult>;

	public sealed class CommandHandler : CommandHandler<Command>
	{
		private readonly IPublisher _publisher;

		public CommandHandler(
			ILoggerFactory loggerFactory,
			IUnitOfWork unitOfWork,
			IPublisher publisher) : base(loggerFactory, unitOfWork)
		{
			_publisher = publisher;
		}

		protected override async Task<CommandResult> ProcessCommandAsync(Command command, CancellationToken cancellationToken)
		{
			var ratingType = await UnitOfWork.VideoRepository.GetRatingVideoAsync(command.VideoId, command.GetMembershipId(), cancellationToken);
			if (ratingType == null)
			{
				return CommandResult.NotFound();
			}

			await UnitOfWork.VideoRepository.DeleteRatingVideoAsync(command.VideoId, command.GetMembershipId(), cancellationToken);

			await _publisher.Publish(new VideoRatingDeletedEvent(command.VideoId, ratingType.Value, command.Context.Identity), cancellationToken);
			
			return CommandResult.NoContent();
		}
	}
}