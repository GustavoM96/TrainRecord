using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.RegisterUser;

public class RegisterUserCommand : IRequest<ErrorOr<RegisterUserResponse>>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Role Role { get; init; }
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
        var newUser = (user, passwordHash).Adapt<User>();

        await _userRepository.AddAsync(newUser);
        return newUser.Adapt<RegisterUserResponse>();
    }
}
