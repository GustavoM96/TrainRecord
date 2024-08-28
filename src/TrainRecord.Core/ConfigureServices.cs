using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Services.Auth;
using TrainRecord.Infrastructure.Services.Identity;

namespace TrainRecord.Core;

public static class ConfigureServices
{
    public static IServiceCollection AddCoreServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<IhashGenerator, HashGenerator>();
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        var secretKey = configuration.GetValue<string>("Jwt:SecretKey")!;
        var key = Encoding.ASCII.GetBytes(secretKey);

        services
            .AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(j =>
            {
                j.RequireHttpsMetadata = false;
                j.SaveToken = true;
                j.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

        return services;
    }
}
