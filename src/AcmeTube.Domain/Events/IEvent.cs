using MediatR;

namespace AcmeTube.Domain.Events;

public interface IEvent : INotification
{
	public string Type { get; }
}

public record Event(string Type) : IEvent;
