namespace TrainRecord.Infrastructure.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
}