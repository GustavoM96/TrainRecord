using MediatR;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Interfaces;

public interface IEntityBase : IAuditableEntityBase
{
    void ClearDomainEvent();
    public IReadOnlyList<INotification> DomainEvents { get; }
}
