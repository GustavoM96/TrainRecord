namespace TrainRecord.Core.Requests;

public class UpdatePasswordRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string NewPassword { get; init; }
}
