namespace TrainRecord.Core.Interfaces;

public interface IEntityBase<TEntity> : IAuditableEntityBase where TEntity : IEntity
{
    void AddDomainEvent(IDomainEvent<TEntity> domainEvent);
    void ClearDomainEvent();
    public IReadOnlyList<IDomainEvent<TEntity>> DomainEvents { get; }
}
