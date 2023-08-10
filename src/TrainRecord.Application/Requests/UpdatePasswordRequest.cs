namespace TrainRecord.Application.Requests;

public record UpdatePasswordRequest(string Email, string Password, string NewPassword) { }
