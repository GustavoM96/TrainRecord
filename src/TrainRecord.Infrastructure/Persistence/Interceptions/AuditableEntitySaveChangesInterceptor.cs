using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Extentions;

namespace TrainRecord.Infrastructure.Persistence.Interceptions
{
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IPublisher _publisher;

        public AuditableEntitySaveChangesInterceptor(
            ICurrentUserService currentUserService,
            IPublisher publisher
        )
        {
            _currentUserService = currentUserService;
            _publisher = publisher;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result
        )
        {
            var entitiesEntry = GetEntities(eventData.Context);
            UpdateEntities(entitiesEntry);
            PublishEvents(entitiesEntry);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default
        )
        {
            var entitiesEntry = GetEntities(eventData.Context);
            UpdateEntities(entitiesEntry);
            PublishEvents(entitiesEntry);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static IEnumerable<EntityEntry<IAuditableEntityBase>> GetEntities(
            DbContext? context
        )
        {
            return context == null
                ? Enumerable.Empty<EntityEntry<IAuditableEntityBase>>()
                : context.ChangeTracker.Entries<IAuditableEntityBase>();
        }

        private void PublishEvents(IEnumerable<EntityEntry<IAuditableEntityBase>> entitiesEntry)
        {
            var entities = entitiesEntry.Select(e => e.Entity).ToList();

            var notifications = entities.SelectMany(entity => entity.DomainEvents).ToList();
            entities.ForEach(entity => entity.ClearDomainEvent());

            foreach (var notification in notifications)
            {
                _publisher.Publish(notification);
            }
        }

        private void UpdateEntities(IEnumerable<EntityEntry<IAuditableEntityBase>> entitiesEntry)
        {
            foreach (var entry in entitiesEntry)
            {
                var user = _currentUserService;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreatedInfo(user.UserId!, DateTime.Now);
                }

                if (entry.AddedOrModified() || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.SetUpdatedInfo(user.UserId!, DateTime.Now);
                }
            }
        }
    }
}
