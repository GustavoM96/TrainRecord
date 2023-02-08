using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Enum;

namespace TrainRecord.Core.Entities;

public class User : AuditableEntityBase
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Role Role { get; init; } = Role.User;
}
