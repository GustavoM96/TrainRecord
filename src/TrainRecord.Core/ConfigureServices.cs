using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Services.Auth;

namespace TrainRecord.Core
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddCoreServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddScoped<IGenaratorHash, GenaratorHash>();
            services.AddScoped<IGenaratorToken, GenaratorToken>();

            var secretKey = configuration.GetSection("Jwt").GetSection("SecretKey").Value;
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
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
