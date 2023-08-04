using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Commum.Bases;

public abstract class EntityBase<TEntity> : AuditableEntityBase, IEntityBase<TEntity>
    where TEntity : IEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public EntityId<TEntity> EntityId => new(Id);
    private readonly List<IDomainEvent<TEntity>> _domainEvents = new();
    public IReadOnlyList<IDomainEvent<TEntity>> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent<TEntity> domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }
}
