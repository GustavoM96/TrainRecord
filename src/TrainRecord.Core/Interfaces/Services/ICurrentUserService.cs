namespace TrainRecord.Core.Interfaces;

public interface ICurrentUserService
{
    string? UserEmail { get; }
    string? UserId { get; }
    string? Role { get; }
    bool IsAdmin { get; }
    bool IsResourceOwner { get; }
    string? GetUserIdByRoute();
}
