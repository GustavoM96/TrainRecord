using TrainRecord.Core.Enum;

namespace TrainRecord.Application.Responses;

public class RegisterUserResponse
{
    public Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Role Role { get; init; }
}
