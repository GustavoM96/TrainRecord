using System.Text.Json.Serialization;
using MediatR;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Commum.Bases;

public abstract class EntityBase<TEntity> : AuditableEntityBase, IEntityBase
    where TEntity : IEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [JsonIgnore]
    public EntityId<TEntity> EntityId => new(Id);

    private readonly List<INotification> _domainEvents = new();

    [JsonIgnore]
    public IReadOnlyList<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvent()
    {
        _domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent<TEntity> domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
