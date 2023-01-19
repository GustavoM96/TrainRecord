using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.LoginUser;

public class LoginUserCommand : IRequest<ErrorOr<LoginUserResponse>>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<LoginUserResponse>>
{
    private readonly IGenaratorHash _genaratorHash;
    private readonly IGenaratorToken _genaratorToken;
    public readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(
        IGenaratorHash genaratorHash,
        IGenaratorToken genaratorToken,
        IUserRepository userRepository
    )
    {
        _genaratorHash = genaratorHash;
        _genaratorToken = genaratorToken;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<LoginUserResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userFound = await _userRepository.GetByEmailAsync(request.Email);
        if (userFound is null)
        {
            return UserError.LoginInvalid;
        }

        var verificationResult = _genaratorHash.VerifyHashedPassword(
            userFound,
            request.Password,
            userFound.Password
        );

        if (verificationResult.Equals(PasswordVerificationResult.Failed))
        {
            return UserError.LoginInvalid;
        }

        if (verificationResult.Equals(PasswordVerificationResult.SuccessRehashNeeded))
        {
            var userWithRehashedPassword = _genaratorHash.SetUserWithRehashedPassword(userFound);
            _userRepository.Update(userWithRehashedPassword);
        }

        var token = _genaratorToken.Generate(userFound);
        return new LoginUserResponse() { IdToken = token };
    }
}
