using ErrorOr;
using LaDeak.JsonMergePatch.Abstractions;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.UserCommand;

public class UpdateUserCommand : IRequest<ErrorOr<RegisterUserResponse>>
{
    public required Patch<User> Patch { get; init; }
    public required Guid UserId { get; init; }
}

public class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, ErrorOr<RegisterUserResponse>>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<RegisterUserResponse>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return UserError.NotFound;
        }

        var updatedUser = request.Patch.ApplyPatch(user);
        if (
            user.Password != updatedUser.Password
            || user.Email != updatedUser.Email
            || user.Id != updatedUser.Id
        )
        {
            return UserError.UpdateInvalid;
        }

        _userRepository.Update(updatedUser);
        return updatedUser.Adapt<RegisterUserResponse>();
    }
}
