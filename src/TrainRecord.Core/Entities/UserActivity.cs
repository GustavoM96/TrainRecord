using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Entities;

public class UserActivity : AuditableEntityBase<UserActivity>
{
    public Guid UserId { get; init; }
    public Guid ActivityId { get; init; }
    public int Weight { get; init; }
    public int Repetition { get; init; }
    public int Serie { get; init; }
}
