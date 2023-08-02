using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Entities;

public class TeacherStudent : AuditableEntityBase
{
    public Guid TeacherId { get; init; }
    public Guid StudentId { get; init; }
}
