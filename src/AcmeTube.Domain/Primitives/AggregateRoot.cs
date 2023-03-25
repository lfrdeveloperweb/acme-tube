using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcmeTube.Domain.Events;

namespace AcmeTube.Domain.Primitives
{
    public abstract class AggregateRoot
    {
        private readonly Collection<IDomainEvent> _domainEvents = new();

        /// <summary>
        /// Gets the domain events. This collection is readonly.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Clears all the domain events from the <see cref="AggregateRoot"/>.
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();

        /// <summary>
        /// Adds the specified <see cref="IDomainEvent"/> to the <see cref="AggregateRoot"/>.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }
}
