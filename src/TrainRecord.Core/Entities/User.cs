using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Core.Entities;

public class User : EntityBase<User>, IEntity
{
    public string Email { get; init; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Role Role { get; init; } = Role.User;

    public void UpdateName(string newFirstName, string newLastName)
    {
        FirstName = newFirstName;
        LastName = newLastName;
    }

    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
    }
}
