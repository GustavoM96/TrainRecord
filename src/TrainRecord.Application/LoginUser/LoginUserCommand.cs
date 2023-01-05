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
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.LoginUser;

public class LoginUserCommand : IRequest<ErrorOr<LoginUserResponse>>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<LoginUserResponse>>
{
    private readonly DbSet<User> _userDbSet;
    private readonly IGenaratorHash _genaratorHash;

    public AppDbContext _context { get; }

    public LoginUserCommandHandler(AppDbContext context, IGenaratorHash genaratorHash)
    {
        _context = context;
        _genaratorHash = genaratorHash;
        _userDbSet = context.Set<User>();
    }

    public async Task<ErrorOr<LoginUserResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userFound = await _userDbSet.SingleOrDefaultAsync(u => u.Email == request.Email);
        if (userFound is null)
        {
            return UserError.LoginInvalid;
        }

        var verificationResult = _genaratorHash.VerifyHashedPassword(
            userFound,
            request.Password,
            userFound.Password
        );

        if (verificationResult.Equals(PasswordVerificationResult.Failed))
        {
            return UserError.LoginInvalid;
        }

        if (verificationResult.Equals(PasswordVerificationResult.SuccessRehashNeeded))
        {
            UpdateHashedPassword(userFound);
        }

        return new LoginUserResponse() { IdToken = "id" };
    }

    private User UpdateHashedPassword(User user)
    {
        var reHashedPassword = _genaratorHash.Generate(user);
        var updatedUser = (user, reHashedPassword).Adapt<User>();
        _userDbSet.Update(updatedUser);

        return updatedUser;
    }
}
