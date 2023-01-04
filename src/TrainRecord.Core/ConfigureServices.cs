using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TrainRecord.Core.Services.Auth;

namespace TrainRecord.Core
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IGenaratorHash, GenaratorHash>();
            return services;
        }
    }
}
