namespace TrainRecord.Application.Requests;

public class UpdateUserRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
