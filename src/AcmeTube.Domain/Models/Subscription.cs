using System;

namespace AcmeTube.Domain.Models;

public sealed class Subscription
{
	public Channel Channel { get; set; }
	public User User { get; set; }
	public DateTimeOffset CreatedAt { get; set; }
}