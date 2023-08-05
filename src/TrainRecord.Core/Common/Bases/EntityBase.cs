using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Commum.Bases;

public abstract class EntityBase<TEntity> : AuditableEntityBase, IEntityBase where TEntity : IEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public EntityId<TEntity> EntityId => new(Id);
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }
}
