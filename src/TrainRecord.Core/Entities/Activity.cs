using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Entities;

public class Activity : EntityBase<Activity>, IEntity
{
    public string Name { get; init; } = null!;
}
