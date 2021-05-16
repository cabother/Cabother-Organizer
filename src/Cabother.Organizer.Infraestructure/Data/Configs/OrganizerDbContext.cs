using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cabother.Organizer.Domain.Entities;
using Cabother.Organizer.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Cabother.Organizer.Infraestructure.Data.Configs
{
    public class OrganizerDbContext : DbContext
    {
        static OrganizerDbContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<TeamStatus>($"{Schema}.team_status");
        }

        public OrganizerDbContext(DbContextOptions<OrganizerDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
        }

        private readonly IMediator _mediator;
        public const string Schema = "organizer";

        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<TeamStatus>(Schema, "team_status");

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        private void UpdatedDateEntity()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity<Guid> &&
                            x.State != EntityState.Deleted).ToList();

            var dateTime = DateTimeOffset.UtcNow;

            entries.AsParallel().ForAll(entry =>
            {
                var baseEntity = entry.Entity as BaseEntity<Guid>;

                if (baseEntity is null)
                    return;

                baseEntity.UpdatedAt = dateTime;

                if (entry.State == EntityState.Added)
                    baseEntity.CreatedAt = dateTime;
            });
        }

        private List<BaseDomainEvent> GetEvents()
        {
            var events = new List<BaseDomainEvent>();

            if (_mediator == null)
                return events;

            var entities = ChangeTracker.Entries<BaseEntity<Guid>>()
                .Where(x => x.Entity.Events.Any())
                .Select(x => x.Entity)
                .ToArray();

            if (!entities.Any())
                return events;

            foreach (var entity in entities)
            {
                events.AddRange(entity.Events.ToArray());
                entity.Events.Clear();
            }

            return events;
        }
        private async Task SendEventsAsync(List<BaseDomainEvent> events)
        {
            if (_mediator == null || events?.Any() != true)
                return;

            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
            }
        }

        #region SaveChanges

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdatedDateEntity();
            var events = GetEvents();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            await SendEventsAsync(events);

            return result;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdatedDateEntity();
            var events = GetEvents();

            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            SendEventsAsync(events).GetAwaiter().GetResult();

            return result;
        }

        #endregion
    }
}