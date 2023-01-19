using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Entities;

public class UserActivity : BaseAuditableEntity
{
    public Guid UserId { get; init; }
    public Guid ActivityId { get; init; }
    public int Weight { get; init; }
    public int Repetition { get; init; }
    public int Serie { get; init; }
}
