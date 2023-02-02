using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Entities;

public class TeacherStudent : BaseAuditableEntity
{
    public Guid TeacherUserId { get; init; }
    public Guid StudentUserId { get; init; }
}
