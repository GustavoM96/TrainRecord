using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Persistence.Interceptions;
using TrainRecord.Infrastructure.Repositories;

namespace TrainRecord.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastuctureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IUserActivityRepository, UserActivityRepository>();
        services.AddScoped<IRepositoryContext, RepositoryContext>();

        var conn = configuration.GetSection("ConnectionStrings").GetSection("DbSqlite").Value;
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(conn);
        });

        return services;
    }
};
