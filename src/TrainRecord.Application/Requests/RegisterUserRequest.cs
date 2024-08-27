using TrainRecord.Core.Enum;

namespace TrainRecord.Application.Requests;

public record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    Role Role
) { }
