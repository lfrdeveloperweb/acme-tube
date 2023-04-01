using AcmeTube.Domain.Models;
using AcmeTube.Domain.Security;

namespace AcmeTube.Domain.Events
{
	public sealed record VideoViewedEvent(string VideoId, IIdentityContext Identity) : Event(Type: "video.viewed");

	public sealed record VideoLikedEvent(string VideoId, bool IncreaseDislikesCount, IIdentityContext Identity) : Event(Type: "video.liked");

	public sealed record VideoDislikedEvent(string VideoId, bool DecreaseLikesCount, IIdentityContext Identity) : Event(Type: "video.disliked");

    public sealed record VideoRatingDeletedEvent(string VideoId, VideoRatingType RatingType, IIdentityContext Identity) : Event(Type: "video-rating.deleted");
}
