using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Interfaces.Repositories;

namespace TrainRecord.Application.UserQuery;

public record GetUserByIdQuery(EntityId<User> UserId) : IRequest<ErrorOr<RegisterUserResponse>> { }

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, ErrorOr<RegisterUserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<RegisterUserResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return UserError.NotFound;
        }

        return user.Adapt<RegisterUserResponse>();
    }
}
