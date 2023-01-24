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
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.GetUserByIdQuery;

public class GetUserByIdQuery : IRequest<ErrorOr<RegisterUserResponse>>
{
    public Guid UserId { get; init; }
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
