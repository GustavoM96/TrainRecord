using TrainRecord.Core.Enum;

namespace TrainRecord.Application.Responses;

public record RegisterUserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    Role Role
) { }
