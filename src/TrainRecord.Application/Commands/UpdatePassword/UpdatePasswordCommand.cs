using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;

namespace TrainRecord.Application.UpdatePassword;

public class UpdatePasswordCommand : IRequest<ErrorOr<Updated>>
{
    public string Email { get; init; }
    public string NewPassword { get; init; }
}

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, ErrorOr<Updated>>
{
    private readonly IGenaratorHash _genaratorHash;
    public readonly IUserRepository _userRepository;

    public UpdatePasswordCommandHandler(
        IGenaratorHash genaratorHash,
        IUserRepository userRepository
    )
    {
        _genaratorHash = genaratorHash;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user is null)
        {
            return UserError.EmailExists;
        }

        var userWithNewPassword = (user, request.NewPassword).Adapt<User>();
        var hashedNewPassword = _genaratorHash.Generate(userWithNewPassword);

        _userRepository.UpdatePasswordById(hashedNewPassword, user.Id);
        return Result.Updated;
    }
}