using System;
using System.Collections.Generic;

namespace AcmeTube.Domain.Models;

public sealed class Video : EntityBase, ICloneable
{
	public string Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public Channel Channel { get; set; }
	public int Priority { get; set; }
	public ICollection<string> Tags { get; set; }
	public VideoStats Stats { get; set; }

	public object Clone() => this.MemberwiseClone();
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

	public string VideoId { get; set; }

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