using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Events.AuthEvents;

namespace TrainRecord.Application.AuthCommand;

public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    Role Role
) : IRequest<ErrorOr<RegisterUserResponse>> { }

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResponse>>
{
    private readonly IGenaratorHash _genaratorHash;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public RegisterUserCommandHandler(
        IGenaratorHash genaratorHash,
        IUserRepository userRepository,
        ICurrentUserService currentUserService
    )
    {
        _genaratorHash = genaratorHash;
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

        var passwordHash = _genaratorHash.Generate(user);
        var newUser = user.UpdateNewUserPassword(passwordHash);

        newUser.AddDomainEvent(new RegisterUserEvent() { Email = newUser.Email });

        await _userRepository.AddAsync(newUser);
        return newUser.Adapt<RegisterUserResponse>();
    }
}
