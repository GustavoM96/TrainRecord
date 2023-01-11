namespace TrainRecord.Infrastructure.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserIdFromRoute { get; }
    string? Role { get; }
    bool IsAdmin { get; }
    bool IsOwnerResource { get; }
}
