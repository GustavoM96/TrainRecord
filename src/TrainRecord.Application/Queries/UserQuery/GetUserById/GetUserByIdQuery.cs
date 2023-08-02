using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;

using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

using TrainRecord.Application.Responses;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.UserQuery;

public class GetUserByIdQuery : IRequest<ErrorOr<RegisterUserResponse>>
{
    public required EntityId<User> UserId { get; init; }
}

public class GetByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<RegisterUserResponse>>
{
    public readonly IUserRepository _userRepository;

    public GetByIdQueryHandler(IUserRepository userRepository)
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
