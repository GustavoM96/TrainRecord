using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Application.RegisterUser;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.GetAllUserQuery;

public class GetAllUserQuery : IRequest<ErrorOr<Page<RegisterUserResponse>>>
{
    public Pagination Pagination { get; init; }
    public Role? Role { get; init; }
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
            .AsPageAdapted<User, RegisterUserResponse>(request.Pagination);
    }
}
