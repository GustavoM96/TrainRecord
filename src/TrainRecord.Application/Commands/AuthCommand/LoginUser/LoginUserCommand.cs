using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.AuthCommand;

public record LoginUserCommand(string Email, string Password)
    : IRequest<ErrorOr<LoginUserResponse>> { }

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<LoginUserResponse>>
{
    private readonly IhashGenerator _hashGenerator;
    private readonly IGenaratorToken _genaratorToken;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(
        IhashGenerator hashGenerator,
        IGenaratorToken genaratorToken,
        IUserRepository userRepository
    )
    {
        _hashGenerator = hashGenerator;
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

        var verificationResult = _hashGenerator.VerifyHashedPassword(
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
            var rehashedPassword = _hashGenerator.Generate(userFound);
            await _userRepository.UpdatePasswordById(rehashedPassword, userFound.EntityId);
        }

        var token = _genaratorToken.Generate(userFound);
        return new LoginUserResponse(token.Key, token.ExpiresHours, token.ExpiresDateTime);
    }
}
