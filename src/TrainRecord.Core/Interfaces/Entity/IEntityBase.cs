using MediatR;

namespace TrainRecord.Core.Interfaces;

public interface IEntityBase : IAuditableEntityBase
{
    void ClearDomainEvent();
    public IReadOnlyList<INotification> DomainEvents { get; }
}
