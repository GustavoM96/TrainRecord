using System.Text.Json.Serialization;
using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Events.AuthEvents;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.AuthCommand;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    Role Role
) : IRequest<ErrorOr<RegisterUserResponse>>
{
    [JsonIgnore]
    public string Password { get; init; } = Password;
}

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResponse>>
{
    private readonly IHashGenerator _hashGenerator;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public RegisterUserCommandHandler(
        IHashGenerator hashGenerator,
        IUserRepository userRepository,
        ICurrentUserService currentUserService
    )
    {
        _hashGenerator = hashGenerator;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<RegisterUserResponse>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.Role == Role.Adm && !_currentUserService.IsAdmin)
        {
            return UserError.RegisterAdmInvalid;
        }

        var user = request.Adapt<User>();

        var anyUser = await _userRepository.AnyByEmailAsync(user.Email);
        if (anyUser)
        {
            return UserError.EmailExists;
        }

        var hashedPassword = _hashGenerator.Generate(user.Password);
        user.UpdatePassword(hashedPassword);

        user.AddDomainEvent(new RegisterUserEvent(user.Email));

        await _userRepository.AddAsync(user);
        return user.Adapt<RegisterUserResponse>();
    }
}
