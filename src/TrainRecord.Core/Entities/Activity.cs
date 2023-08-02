using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Entities;

public class Activity : AuditableEntityBase<Activity>
{
    public string Name { get; init; } = null!;
}
