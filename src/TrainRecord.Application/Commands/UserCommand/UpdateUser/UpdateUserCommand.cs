using ErrorOr;
using LaDeak.JsonMergePatch.Abstractions;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Requests;

namespace TrainRecord.Application.UserCommand;

public class UpdateUserCommand : IRequest<ErrorOr<RegisterUserResponse>>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required EntityId<User> UserId { get; init; }
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

        var updatedUser = user.UpdateNewUser(request.FirstName, request.LastName);
        _userRepository.Update(updatedUser);

        return updatedUser.Adapt<RegisterUserResponse>();
    }
}
