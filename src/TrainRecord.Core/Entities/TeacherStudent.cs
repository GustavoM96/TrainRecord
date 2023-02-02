using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Entities;

public class TeacherStudent : BaseAuditableEntity
{
    public Guid TeacherId { get; init; }
    public Guid StudentId { get; init; }
}
