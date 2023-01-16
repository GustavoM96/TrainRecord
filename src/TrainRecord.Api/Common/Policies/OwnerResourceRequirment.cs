using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

            if (isAdmin)
            {
                context.Succeed(requirement);
            }

            if (isOwnerResource)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
