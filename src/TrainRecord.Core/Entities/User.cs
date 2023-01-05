using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Entities;

public class User : BaseAuditableEntity
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
