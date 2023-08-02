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

    public User UpdateNewUser(string newFirstName, string newLastName)
    {
        return new User()
        {
            FirstName = newFirstName,
            LastName = newLastName,
            Email = Email,
            Password = Password,
            Role = Role
        };
    }

    public User UpdateNewUserPassword(string newPassword)
    {
        return new User()
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = newPassword,
            Role = Role
        };
    }
}
