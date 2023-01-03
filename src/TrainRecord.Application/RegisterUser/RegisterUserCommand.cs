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
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Application.RegisterUser;

public class RegisterUserCommand : IRequest<ErrorOr<RegisterUserResponse>>
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}


public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<RegisterUserResponse>>
{
    private readonly DbSet<User> _userDbSet;
    public AppDbContext _context { get; }

    public RegisterUserCommandHandler(AppDbContext context)
    {
        _context = context;
        _userDbSet = context.Set<User>();
    }

    public async Task<ErrorOr<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.Adapt<User>();

        var userFound = await _userDbSet.AnyAsync(u => u.Email == request.Email);
        if (userFound)
        {
            return UserError.EmailExists;
        }

        await _userDbSet.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Adapt<RegisterUserResponse>();
    }
}

