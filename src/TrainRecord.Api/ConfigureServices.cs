using Microsoft.AspNetCore.Authorization;
using TrainRecord.Api.Common.Policies.AdmRequirment;
using TrainRecord.Api.Common.Policies.ResourceOwnerRequirment;

namespace TrainRecord.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, ResourceOwnerHandler>();
        services.AddSingleton<IAuthorizationHandler, AdmHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "ResourceOwner",
                policyBuilder => policyBuilder.AddRequirements(new ResourceOwnerRequirment())
            );
            options.AddPolicy(
                "IsAdm",
                policyBuilder => policyBuilder.AddRequirements(new AdmRequirment())
            );
        });

        return services;
    }
}
