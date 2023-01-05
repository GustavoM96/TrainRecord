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

namespace TrainRecord.Application.RegisterUser;

public class LoginUserCommand : IRequest<ErrorOr<string>>
{
    public string Email { get; init; }
    public string Password { get; init; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ErrorOr<string>>
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

    public async Task<ErrorOr<string>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userFound = await _userDbSet.FindAsync(request.Email);
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

        return "id";
    }

    private User UpdateHashedPassword(User user)
    {
        var reHashedPassword = _genaratorHash.Generate(user);
        var updatedUser = (user, reHashedPassword).Adapt<User>();
        _userDbSet.Update(updatedUser);

        return updatedUser;
    }
}
