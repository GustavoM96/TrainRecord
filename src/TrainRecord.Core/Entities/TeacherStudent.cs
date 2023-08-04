using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Entities;

public class TeacherStudent : EntityBase<TeacherStudent>, IEntity
{
    public Guid TeacherId { get; init; }
    public Guid StudentId { get; init; }
}
