using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Application.Responses;

namespace TrainRecord.Application.UserCommand;

public record UpdateUserCommand(EntityId<User> UserId, string FirstName, string LastName)
    : IRequest<ErrorOr<RegisterUserResponse>> { }

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

        var updatedUser = user.UpdateNewUser(request.FirstName, request.LastName);
        _userRepository.Update(updatedUser);

        return updatedUser.Adapt<RegisterUserResponse>();
    }
}
