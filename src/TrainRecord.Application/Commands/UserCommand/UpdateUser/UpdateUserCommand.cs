using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

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

        user.UpdateName(request.FirstName, request.LastName);
        _userRepository.Update(user);

        return user.Adapt<RegisterUserResponse>();
    }
}
