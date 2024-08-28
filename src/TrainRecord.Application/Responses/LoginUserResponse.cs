namespace TrainRecord.Application.Responses;

public record LoginUserResponse(string IdToken, TimeSpan ExpiresTime, DateTime ExpiresDateTime) { }
