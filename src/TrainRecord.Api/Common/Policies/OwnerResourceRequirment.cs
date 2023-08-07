using Microsoft.AspNetCore.Authorization;
using Throw;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Exceptions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Api.Common.Policies.OwnerResourceRequirment;

public class OwnerResourceRequirment : IAuthorizationRequirement { }

public class OwnerResourceHandler : AuthorizationHandler<OwnerResourceRequirment>
{
    private readonly ICurrentUserService _currentUserService;

    public OwnerResourceHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OwnerResourceRequirment requirement
    )
    {
        _currentUserService
            .Throw(() => new AuthorizationException(UserError.IsNotOwnerResourceNorAdm))
            .IfFalse(u => u.IsOwnerResource || u.IsAdmin);

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
