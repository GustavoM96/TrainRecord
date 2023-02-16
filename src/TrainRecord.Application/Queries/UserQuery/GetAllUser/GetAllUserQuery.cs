using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Filter;
using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Requests;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.UserQuery;

public class GetAllUserQuery : IRequest<ErrorOr<Page<RegisterUserResponse>>>
{
    public Pagination Pagination { get; init; }
    public Role? Role { get; init; }
    public UserQueryRequest UserQueryRequest { get; init; }
}

public class GetAllUserQueryHandler
    : IRequestHandler<GetAllUserQuery, ErrorOr<Page<RegisterUserResponse>>>
{
    public readonly IUserRepository _userRepository;

    public GetAllUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Page<RegisterUserResponse>>> Handle(
        GetAllUserQuery request,
        CancellationToken cancellationToken
    )
    {
        return _userRepository
            .GetAllByRole(request.Role)
            .Apply(request.UserQueryRequest)
            .AsPageAdapted<User, RegisterUserResponse>(request.Pagination);
    }
}
