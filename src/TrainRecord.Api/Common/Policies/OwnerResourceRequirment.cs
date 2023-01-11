using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TrainRecord.Infrastructure.Interfaces;

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
            var userId = _currentUserService.UserId;
            var userIdFromRoute = _currentUserService.UserIdFromRoute;

            if (userId is null || userIdFromRoute is null)
            {
                return Task.CompletedTask;
            }

            if (userId == userIdFromRoute)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
