using System;

namespace AcmeTube.Domain.Models;

public sealed class Subscription
{
	//public string ChannelId { get; set; }
	public Channel Channel { get; set; }
	//public string MembershipId { get; set; }
	public User User { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
}