using TrainRecord.Core.Commum;

namespace TrainRecord.Core.Entities;

public class User : BaseAuditableEntity
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
