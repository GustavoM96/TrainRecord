using AspNetCore.IQueryable.Extensions;
using ErrorOr;
using MediatR;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Application.Requests;
using TrainRecord.Application.Responses;

namespace TrainRecord.Application.UserQuery;

public record GetAllUserQuery(UserQueryRequest? UserQueryRequest, Pagination Pagination)
    : IRequest<ErrorOr<Page<RegisterUserResponse>>> { }

public class GetAllUserQueryHandler
    : IRequestHandler<GetAllUserQuery, ErrorOr<Page<RegisterUserResponse>>>
{
    private readonly IUserRepository _userRepository;

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
            .AsNoTracking()
            .Apply(request.UserQueryRequest)
            .AsPageAdapted<User, RegisterUserResponse>(request.Pagination);
    }
}
