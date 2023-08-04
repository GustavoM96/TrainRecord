using MediatR;

namespace TrainRecord.Core.Interfaces;

public interface IDomainEvent<IEntity> : INotification { }
