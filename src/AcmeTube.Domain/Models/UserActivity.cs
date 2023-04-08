namespace AcmeTube.Domain.Models;

public sealed class UserActivity
{
	public string Id { get; set; }
	public string EventType { get; set; }
	public string EventData { get; set; }
	public User User { get; set; }
}