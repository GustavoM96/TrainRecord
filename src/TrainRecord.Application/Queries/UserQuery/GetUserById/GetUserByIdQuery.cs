using ErrorOr;
using Mapster;
using MediatR;
using TrainRecord.Application.Errors;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Application.Responses;

namespace TrainRecord.Application.UserQuery;

public class GetUserByIdQuery : IRequest<ErrorOr<RegisterUserResponse>>
{
    public required Guid UserId { get; init; }
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
