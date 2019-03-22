using System;
using System.Collections.Generic;

namespace Demo.Domain.Shared
{
    public interface IEntity
    {
        string Id { get; set; }
    }

    public abstract class AggregateRoot : IEntity
    {
        private readonly List<DomainEvent> _uncommittedEvents = new List<DomainEvent>();
        public IReadOnlyCollection<DomainEvent> UncommittedEvents => _uncommittedEvents;

        public string Id { get; set; }

        protected void Raise<T>(Action<T> ctor)
            where T : DomainEvent
        {
            var instance = Activator.CreateInstance<T>();
            ctor(instance);

            _uncommittedEvents.Add(instance);
        }

        public void ClearEvents()
        {
            _uncommittedEvents.Clear();
        }
    }
}
