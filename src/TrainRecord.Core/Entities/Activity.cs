using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Entities;

public class Activity : AuditableEntityBase
{
    public string Name { get; init; } = null!;
}
