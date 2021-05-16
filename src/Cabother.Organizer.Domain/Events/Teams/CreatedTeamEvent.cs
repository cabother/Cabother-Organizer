using System;
using Cabother.Organizer.Domain.Entities;

namespace Cabother.Organizer.Domain.Events.Teams
{
    public class CreatedTeamEvent : BaseDomainEvent
    {
        public CreatedTeamEvent(Team createdTeam)
        {
            CreatedTeam = createdTeam ?? throw new ArgumentNullException(nameof(createdTeam));
        }

        public Team CreatedTeam { get; }
    }
}