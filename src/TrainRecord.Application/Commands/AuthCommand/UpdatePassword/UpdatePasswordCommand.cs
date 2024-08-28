using System.Text.Json.Serialization;
using ErrorOr;
using MediatR;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.AuthCommand;

public record UpdatePasswordCommand(string Email, string Password, string NewPassword)
    : IRequest<ErrorOr<Updated>>
{
    [JsonIgnore]
    public string Password { get; init; } = Password;

    [JsonIgnore]
    public string NewPassword { get; init; } = NewPassword;
}

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, ErrorOr<Updated>>
{
    private readonly IHashGenerator _hashGenerator;
    private readonly IUserRepository _userRepository;

    public UpdatePasswordCommandHandler(
        IHashGenerator hashGenerator,
        IUserRepository userRepository
    )
    {
        _hashGenerator = hashGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var hashedPassword = _hashGenerator.Generate(request.NewPassword);

        await _userRepository.UpdatePasswordByEmail(request.Email, hashedPassword);
        return Result.Updated;
    }
}
