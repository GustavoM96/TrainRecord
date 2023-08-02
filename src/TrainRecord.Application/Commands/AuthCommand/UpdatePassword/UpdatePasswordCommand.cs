using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.AuthCommand;

public class UpdatePasswordCommand : IRequest<ErrorOr<Updated>>
{
    public required string Email { get; init; }
    public required string NewPassword { get; init; }
}

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, ErrorOr<Updated>>
{
    private readonly IGenaratorHash _genaratorHash;
    public readonly IUserRepository _userRepository;

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

        var userWithNewPassword = (user, request.NewPassword).Adapt<User>();
        var hashedNewPassword = _genaratorHash.Generate(userWithNewPassword);

        await _userRepository.UpdatePasswordById(hashedNewPassword, user.EntityId);
        return Result.Updated;
    }
}
