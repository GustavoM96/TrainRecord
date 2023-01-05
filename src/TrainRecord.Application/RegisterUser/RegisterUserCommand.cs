using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.RegisterUser;

public class RegisterUserCommand : IRequest<ErrorOr<RegisterUserResponse>>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}

public class RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResponse>>
{
    private readonly DbSet<User> _userDbSet;
    private readonly IGenaratorHash _genaratorHash;

    public AppDbContext _context { get; }

    public RegisterUserCommandHandler(AppDbContext context, IGenaratorHash genaratorHash)
    {
        _context = context;
        _genaratorHash = genaratorHash;
        _userDbSet = context.Set<User>();
    }

    public async Task<ErrorOr<RegisterUserResponse>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = request.Adapt<User>();

        var userFound = await _userDbSet.AnyAsync(u => u.Email == user.Email);
        if (userFound)
        {
            return UserError.EmailExists;
        }

        var passwordHash = _genaratorHash.Generate(user);
        var newUser = (user, passwordHash).Adapt<User>();

        await _userDbSet.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser.Adapt<RegisterUserResponse>();
    }
}
