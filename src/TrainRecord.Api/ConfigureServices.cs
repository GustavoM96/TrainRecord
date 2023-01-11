using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using TrainRecord.Api.Common.Policies.OwnerResourceRequirment;

namespace TrainRecord.Api
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, OwnerResourceHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "OwnerResource",
                    policyBuilder => policyBuilder.AddRequirements(new OwnerResourceRequirment())
                );
            });

            return services;
        }
    }
}
