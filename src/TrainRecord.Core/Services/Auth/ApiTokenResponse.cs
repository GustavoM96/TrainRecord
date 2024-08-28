namespace TrainRecord.Core.Services.Auth;

public record ApiTokenResponse(TimeSpan Expire)
{
    public DateTime ExpiresDateTime = DateTime.Now.Add(Expire);
    public string Key { get; set; } = "";
}
