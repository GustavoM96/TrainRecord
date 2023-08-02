namespace TrainRecord.Application.Requests;

public class UpdatePasswordRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string NewPassword { get; init; }
}
