using Microsoft.AspNetCore.Authorization;
using TrainRecord.Application.Errors;
using TrainRecord.Core.Exceptions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Api.Common.Policies.OwnerResourceRequirment
{
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
            var isAdmin = _currentUserService.IsAdmin;
            var isOwnerResource = _currentUserService.IsOwnerResource;

            if (!isOwnerResource && !isAdmin)
            {
                throw new AuthorizationException(UserError.IsNotOwnerResourceAndAdm);
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
