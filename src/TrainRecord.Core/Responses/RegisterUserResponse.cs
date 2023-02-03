using TrainRecord.Core.Enum;

namespace TrainRecord.Core.Responses;

public class RegisterUserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Role Role { get; init; }
}
