namespace TrainRecord.Core.Services.Auth;

public record ApiTokenResponse(int ExpiresHours)
{
    public DateTime ExpiresDateTime = DateTime.Now.AddHours(ExpiresHours);
    public string Key { get; set; } = "";
}
