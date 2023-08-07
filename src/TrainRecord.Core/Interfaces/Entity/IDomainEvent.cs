using MediatR;

namespace TrainRecord.Core.Interfaces;

public interface IDomainEvent<TEntity> : INotification where TEntity : IEntity { }
