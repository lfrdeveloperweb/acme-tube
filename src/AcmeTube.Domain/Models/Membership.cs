using System;
using AcmeTube.Domain.Security;

namespace AcmeTube.Domain.Models;

public class Membership : EntityBase
{
	public string Id { get; set; }

	public required string Name { get; set; }

	public Role Role { get; set; }

	public DateTimeOffset? LockedAt { get; protected set; }

	public bool IsLocked => LockedAt.HasValue;
}