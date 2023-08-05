using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Interfaces;

public interface IEntityBase : IAuditableEntityBase
{
    void AddDomainEvent(IDomainEvent domainEvent);
    void ClearDomainEvent();
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
}
