using System;
using System.Collections.Generic;

namespace AcmeTube.Domain.Models;

public enum VideoRatingType : byte
{
	Dislike = 0,
	Like = 1
}

public sealed class Video : EntityBase
{
	public string Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public Channel Channel { get; set; }
	public TimeSpan Duration { get; set; }
	public bool IsPublic { get; set; }
	public string VideoFilePath { get; set; }
	public string VideoUrl { get; set; }
	public string VideoExternalId { get; set; }
	public string ThumbnailFilePath { get; set; }
	public string ThumbnailUrl { get; set; }
	public string ThumbnailExternalId { get; set; }
	public ICollection<string> Tags { get; set; }
	public VideoStats Stats { get; set; }

	public void IncreaseViewsCount() => Stats.ViewsCount++;
	
	public void IncreaseLikesCount() => Stats.LikesCount++;

	public void DecreaseLikesCount()
	{
		if (Stats.LikesCount > 0)
			Stats.LikesCount--;
	}

	public void IncreaseDislikesCount() => Stats.DislikesCount++;

	public void DecreaseDislikesCount()
	{
		if (Stats.DislikesCount > 0)
			Stats.DislikesCount--;
	}

	public void IncreaseCommentsCount() => Stats.CommentsCount++;

	public void DecreaseCommentsCount()
	{
		if (Stats.CommentsCount > 0)
			Stats.CommentsCount--;
	}
}

public sealed class VideoStats
{
	public int ViewsCount { get; set; }
	public int LikesCount { get; set; }
	public int DislikesCount { get; set; }
	public int CommentsCount { get; set; }
}

public sealed class VideoComment
{
	public int Id { get; set; }
	public Video Video { get; set; }
	public string Description { get; set; }

	/// <summary>
	/// Identifier of user that created the record.
	/// </summary>
	public string CreatedBy { get; set; }

	/// <summary>
	/// Date and time of record creation.
	/// </summary>
	public DateTimeOffset CreatedAt { get; set; }

}