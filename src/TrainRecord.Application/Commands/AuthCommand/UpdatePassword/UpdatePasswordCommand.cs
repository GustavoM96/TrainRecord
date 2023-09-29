using ErrorOr;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Interfaces;
using TrainRecord.Application.Interfaces.Repositories;

namespace TrainRecord.Application.AuthCommand;

public record UpdatePasswordCommand(string Email, string NewPassword)
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
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
        {
            return UserError.EmailExists;
        }

        var userWithNewPassword = user.UpdateNewUserPassword(request.NewPassword);
        var hashedNewPassword = _genaratorHash.Generate(userWithNewPassword);

        await _userRepository.UpdatePasswordById(hashedNewPassword, user.EntityId);
        return Result.Updated;
    }
}
