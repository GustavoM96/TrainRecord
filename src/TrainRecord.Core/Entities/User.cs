using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Enum;

namespace TrainRecord.Core.Entities;

public class User : AuditableEntityBase<User>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public Role Role { get; init; } = Role.User;
}
