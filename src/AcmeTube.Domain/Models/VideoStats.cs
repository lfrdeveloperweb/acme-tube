namespace AcmeTube.Domain.Models;

public sealed class VideoStats
{
	public int ViewsCount { get; set; }
	public int LikesCount { get; set; }
	public int DislikesCount { get; set; }
	public int CommentsCount { get; set; }
}