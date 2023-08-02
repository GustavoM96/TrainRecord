namespace TrainRecord.Core.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Role { get; }
    bool IsAdmin { get; }
    bool IsOwnerResource { get; }
    string? GetUserIdByRoute();
}
