using Microsoft.AspNetCore.Authorization;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Exceptions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Api.Common.Policies.AdmRequirment;

public class AdmRequirment : IAuthorizationRequirement { }

public class AdmHandler : AuthorizationHandler<AdmRequirment>
{
    private readonly ICurrentUserService _currentUserService;

    public AdmHandler(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdmRequirment requirement
    )
    {
        if (!_currentUserService.IsAdmin)
        {
            throw new AuthorizationException(UserError.IsNotAdm);
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
