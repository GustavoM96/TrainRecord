namespace TrainRecord.Core.Requests;

public class UpdatePasswordRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string NewPassword { get; init; }
}
