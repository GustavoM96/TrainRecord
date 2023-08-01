using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.AuthCommand;

public class LoginUserCommand : IRequest<ErrorOr<LoginUserResponse>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
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
            var rehashedPassword = _genaratorHash.Generate(userFound);
            await _userRepository.UpdatePasswordById(rehashedPassword, userFound.Id);
        }

        var token = _genaratorToken.Generate(userFound);
        return new LoginUserResponse() { IdToken = token };
    }
}
