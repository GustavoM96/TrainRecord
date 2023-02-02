using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using LaDeak.JsonMergePatch.Abstractions;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.UpdateUser;

public class UpdateUserCommand : IRequest<ErrorOr<RegisterUserResponse>>
{
    public Patch<User> Patch { get; init; }
    public Guid UserId { get; init; }
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

        var updatedUser = request.Patch.ApplyPatch(user);
        if (
            user.Password != updatedUser.Password
            || user.Email != updatedUser.Email
            || user.Id != updatedUser.Id
        )
        {
            return UserError.UpdateInvalid;
        }

        _userRepository.Detached(user);
        _userRepository.Update(updatedUser);
        return updatedUser.Adapt<RegisterUserResponse>();
    }
}
