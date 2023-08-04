using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Entities;

public class UserActivity : AuditableEntityBase<UserActivity>, IEntity
{
    public Guid UserId { get; init; }
    public Guid ActivityId { get; init; }
    public int Weight { get; init; }
    public int Repetition { get; init; }
    public int Serie { get; init; }
}
