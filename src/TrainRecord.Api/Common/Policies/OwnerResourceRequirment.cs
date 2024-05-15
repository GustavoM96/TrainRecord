using Microsoft.AspNetCore.Authorization;
using Throw;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Exceptions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Api.Common.Policies.ResourceOwnerRequirment;

public class ResourceOwnerRequirment : IAuthorizationRequirement { }

public class ResourceOwnerHandler : AuthorizationHandler<ResourceOwnerRequirment>
{
    private readonly ICurrentUserService _currentUserService;

    public ResourceOwnerHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceOwnerRequirment requirement
    )
    {
        _currentUserService
            .Throw(() => new AuthorizationException(UserError.IsNotResourceOwnerNorAdm))
            .IfFalse(u => u.IsResourceOwner || u.IsAdmin);

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
