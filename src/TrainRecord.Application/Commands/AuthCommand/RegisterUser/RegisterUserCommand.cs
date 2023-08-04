using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Events.AuthEvents;

namespace TrainRecord.Application.AuthCommand;

public class RegisterUserCommand : IRequest<ErrorOr<RegisterUserResponse>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required Role Role { get; init; }
}

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

        newUser.AddDomainEevnt(new RegisterUserEvent() { Email = newUser.Email });

        await _userRepository.AddAsync(newUser);
        return newUser.Adapt<RegisterUserResponse>();
    }
}
