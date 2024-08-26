namespace TrainRecord.Application.Responses;

public record LoginUserResponse(string IdToken, int ExpiresHours, DateTime ExpiresDateTime) { }
