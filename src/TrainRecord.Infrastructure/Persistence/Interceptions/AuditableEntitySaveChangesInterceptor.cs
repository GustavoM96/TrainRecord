using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Extensions;

namespace TrainRecord.Infrastructure.Persistence.Interceptions;

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
        var entitiesBaseEntry = GetEntitiesBase(eventData.Context);
        PublishEvents(entitiesBaseEntry);
        UpdateEntities(entitiesBaseEntry);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var entitiesBaseEntry = GetEntitiesBase(eventData.Context);
        PublishEvents(entitiesBaseEntry);
        UpdateEntities(entitiesBaseEntry);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static IEnumerable<EntityEntry<IEntityBase>> GetEntitiesBase(DbContext? context)
    {
        return context == null
            ? Enumerable.Empty<EntityEntry<IEntityBase>>()
            : context.ChangeTracker.Entries<IEntityBase>();
    }

    private void PublishEvents(IEnumerable<EntityEntry<IEntityBase>> entitiesEntry)
    {
        var entities = entitiesEntry.Select(e => e.Entity).ToList();
        var notifications = entities.SelectMany(entity => entity.DomainEvents).ToList();

        entities.ForEach(entity => entity.ClearDomainEvent());

        foreach (var notification in notifications)
        {
            _publisher.Publish(notification);
        }
    }

    private void UpdateEntities(IEnumerable<EntityEntry<IEntityBase>> entitiesEntry)
    {
        foreach (var entry in entitiesEntry)
        {
            var user = _currentUserService;
            var now = DateTime.Now;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetCreatedInfo(user.UserId!, now);
            }

            if (entry.AddedOrModified() || entry.HasChangedOwnedEntities())
            {
                entry.Entity.SetUpdatedInfo(user.UserId!, now);
            }
        }
    }
}
