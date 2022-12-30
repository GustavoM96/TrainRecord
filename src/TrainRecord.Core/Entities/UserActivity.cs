using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Entities;

public class UserActivity : BaseAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid ActivityId { get; set; }
    public int Weight { get; set; }
    public int Repetition { get; set; }
}
