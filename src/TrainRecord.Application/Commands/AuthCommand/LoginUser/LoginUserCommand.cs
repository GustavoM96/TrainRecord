using System.Text.Json.Serialization;
using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.AuthCommand;

public record LoginUserCommand(string Email, string Password) : IRequest<ErrorOr<LoginUserResponse>>
{
    [JsonIgnore]
    public string Password { get; set; } = Password;
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<LoginUserResponse>>
{
    private readonly IHashGenerator _hashGenerator;
    private readonly ITokenGenerator _genaratorToken;
    private readonly IUserRepository _userRepository;

    public LoginUserCommandHandler(
        IHashGenerator hashGenerator,
        ITokenGenerator genaratorToken,
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

        var isVerified = _hashGenerator.VerifyHashedPassword(request.Password, userFound.Password);
        if (!isVerified)
        {
            return UserError.LoginInvalid;
        }

        var token = _genaratorToken.Generate(userFound);
        return new LoginUserResponse(token.Key, token.Expire, token.ExpiresDateTime);
    }
}
