using System;
using AcmeTube.Application.Repositories;
using AcmeTube.Domain.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AcmeTube.Domain.Models;

namespace AcmeTube.Application.Features.Videos
{
	internal sealed class VideoEventHandler :
		INotificationHandler<VideoViewedEvent>,
		INotificationHandler<VideoLikedEvent>,
		INotificationHandler<VideoDislikedEvent>,
		INotificationHandler<VideoRatingDeletedEvent>
	{
		private readonly IUnitOfWork _unitOfWork;

		public VideoEventHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Handle(VideoViewedEvent @event, CancellationToken cancellationToken)
		{
			var video = await _unitOfWork.VideoRepository.GetByIdAsync(@event.VideoId, cancellationToken);
			if (@event.Identity.IsAuthenticated)
			{
				if (await _unitOfWork.VideoRepository.IncreaseVideoViewsCountAsync(@event.VideoId, @event.Identity.Id, cancellationToken))
				{
					video.IncreaseViewsCount();
					await _unitOfWork.VideoRepository.UpdateAsync(video, cancellationToken);
				}
			}
			else
			{
				video.IncreaseViewsCount();
				await _unitOfWork.VideoRepository.UpdateAsync(video, cancellationToken);
			}

			await _unitOfWork.CommitTransactionAsync(cancellationToken);
		}

		public async Task Handle(VideoLikedEvent @event, CancellationToken cancellationToken)
		{
			var video = await _unitOfWork.VideoRepository.GetByIdAsync(@event.VideoId, cancellationToken);
			video.IncreaseLikesCount();

			if (@event.IncreaseDislikesCount)
			{
				video.DecreaseDislikesCount();
			}

			await _unitOfWork.VideoRepository.UpdateAsync(video, cancellationToken);
		}

		public async Task Handle(VideoDislikedEvent @event, CancellationToken cancellationToken)
		{
			var video = await _unitOfWork.VideoRepository.GetByIdAsync(@event.VideoId, cancellationToken);
			video.IncreaseDislikesCount();

			if (@event.DecreaseLikesCount)
			{
				video.DecreaseLikesCount();
			}

			await _unitOfWork.VideoRepository.UpdateAsync(video, cancellationToken);
		}

		public async Task Handle(VideoRatingDeletedEvent @event, CancellationToken cancellationToken)
		{
			var video = await _unitOfWork.VideoRepository.GetByIdAsync(@event.VideoId, cancellationToken);
			
			Action videoRatingAction = @event.RatingType == VideoRatingType.Like 
				? video.DecreaseLikesCount 
				: video.DecreaseDislikesCount;

			videoRatingAction();

			await _unitOfWork.VideoRepository.UpdateAsync(video, cancellationToken);
		}
	}
}
