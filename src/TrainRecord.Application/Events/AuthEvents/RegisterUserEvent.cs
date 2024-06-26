using MediatR;
using Microsoft.Extensions.Logging;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.Events.AuthEvents;

public record RegisterUserEvent(string Email) : IDomainEvent<User> { }

public class RegisterUserEvenHandle : INotificationHandler<RegisterUserEvent>
{
    private readonly ILogger<RegisterUserEvenHandle> _logger;

    public RegisterUserEvenHandle(ILogger<RegisterUserEvenHandle> logger)
    {
        _logger = logger;
    }

    public Task Handle(RegisterUserEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Send email to new user {email}", notification.Email);
        return Task.CompletedTask;
    }
}
