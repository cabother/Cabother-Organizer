using System;
using MediatR;

namespace Cabother.Organizer.Domain.Events
{
    public class BaseDomainEvent : INotification
    {
        protected BaseDomainEvent()
        {
            When = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset When { get; }
    }
}