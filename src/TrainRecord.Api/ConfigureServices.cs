using Microsoft.AspNetCore.Authorization;
using TrainRecord.Api.Common.Policies.AdmRequirment;
using TrainRecord.Api.Common.Policies.ResourceOwnerRequirment;
using TrainRecord.Api.Handler;

namespace TrainRecord.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddSingleton<IAuthorizationHandler, ResourceOwnerHandler>();
        services.AddSingleton<IAuthorizationHandler, AdmHandler>();

        services
            .AddAuthorizationBuilder()
            .AddPolicy(
                "ResourceOwner",
                policyBuilder => policyBuilder.AddRequirements(new ResourceOwnerRequirment())
            )
            .AddPolicy(
                "IsAdm",
                policyBuilder => policyBuilder.AddRequirements(new AdmRequirment())
            );

        return services;
    }
}
