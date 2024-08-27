using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Entities;

public class UserActivity : EntityBase<UserActivity>, IEntity
{
    public Guid UserId { get; init; }
    public Guid? TeacherId { get; init; }
    public Guid ActivityId { get; init; }
    public int Weight { get; init; }
    public int Repetition { get; init; }
    public int Serie { get; init; }
    public TimeOnly? Time { get; init; }
    public string? TrainGroup { get; init; }
    public string? TrainName { get; init; }
}
