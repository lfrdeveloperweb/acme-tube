using AcmeTube.Application.Core.Commands;
using AcmeTube.Application.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Events;
using AcmeTube.Domain.Models;
using MediatR;

public sealed class CreateRatingVideo
{
	public sealed record Command(string VideoId, VideoRatingType RatingType) : Command<CommandResult>;

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
			if (await UnitOfWork.VideoRepository.GetByIdAsync(command.VideoId, cancellationToken: cancellationToken) is not { } video)
			{
				return CommandResult.NotFound();
			}

			var ratingType = await UnitOfWork.VideoRepository.GetRatingVideoAsync(command.VideoId, command.GetMembershipId(), cancellationToken);
			if (ratingType.HasValue && ratingType == command.RatingType)
			{
				return CommandResult.NoContent();
			}

			await UnitOfWork.VideoRepository.UpsertRatingVideoAsync(video.Id, command.GetMembershipId(), command.RatingType, cancellationToken);

			IEvent @event = command.RatingType == VideoRatingType.Like
				? new VideoLikedEvent(video.Id, IncreaseDislikesCount: ratingType is VideoRatingType.Dislike, command.Context.Identity)
				: new VideoDislikedEvent(video.Id, DecreaseLikesCount: ratingType is VideoRatingType.Like, command.Context.Identity);

			await _publisher.Publish(@event, cancellationToken);

			return CommandResult.NoContent();
		}
	}
}