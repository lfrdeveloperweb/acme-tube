using System.Collections.Generic;

namespace AcmeTube.Domain.Models;

public abstract class Playlist : EntityBase
{
	public string Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public ICollection<Video> Videos { get; set; }
}

public sealed class ChannelPlaylist : Playlist
{
	public Channel Channel { get; set; }
	public bool? IsPublic { get; set; }
}

public sealed class UserPlaylist : Playlist
{
	public User User { get; set; }
}

public sealed class PlaylistItem
{
	public string Id { get; set; }
	public Channel Channel { get; set; }
	public Video Video { get; set; }
	public int Position { get; set; }
}