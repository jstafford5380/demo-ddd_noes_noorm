using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Domain.Shared;

namespace Demo.Infrastructure.Data
{
    public abstract class EventRepository
    {
        private readonly Dictionary<Type, Func<object, Task>> _handlers = new Dictionary<Type, Func<object, Task>>();
        
        protected void AddHandler<T>(Func<T, Task> handler) 
            where T : DomainEvent
        {
            _handlers.Add(typeof(T), o => handler((T) o));
        }

        protected Task DispatchAsync<T>(T e) where T : DomainEvent
        {
            var messageType = typeof(T);
            return _handlers.ContainsKey(messageType) 
                ? _handlers[typeof(T)](e) 
                : Task.CompletedTask;
        }
    }
}