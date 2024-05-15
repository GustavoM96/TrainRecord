using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Interfaces;
using TrainRecord.Application.Interfaces.Repositories;

namespace TrainRecord.Application.AuthCommand;

public record UpdatePasswordCommand(string Email, string Password, string NewPassword)
    : IRequest<ErrorOr<Updated>> { }

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, ErrorOr<Updated>>
{
    private readonly IGenaratorHash _genaratorHash;
    private readonly IUserRepository _userRepository;

    public UpdatePasswordCommandHandler(
        IGenaratorHash genaratorHash,
        IUserRepository userRepository
    )
    {
        _genaratorHash = genaratorHash;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var hashedPassword = _genaratorHash.Generate(new() { Password = request.Password });

        var hasUpdated = await _userRepository.UpdatePasswordByEmail(
            request.Email,
            request.Password,
            hashedPassword
        );
        return hasUpdated ? Result.Updated : UserError.EmailExists;
    }
}
