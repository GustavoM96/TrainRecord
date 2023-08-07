using Microsoft.AspNetCore.Authorization;
using TrainRecord.Api.Common.Policies.AdmRequirment;
using TrainRecord.Api.Common.Policies.OwnerResourceRequirment;

namespace TrainRecord.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, OwnerResourceHandler>();
        services.AddSingleton<IAuthorizationHandler, AdmHandler>();

        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "OwnerResource",
                policyBuilder => policyBuilder.AddRequirements(new OwnerResourceRequirment())
            );
            options.AddPolicy(
                "IsAdm",
                policyBuilder => policyBuilder.AddRequirements(new AdmRequirment())
            );
        });

        return services;
    }
}
