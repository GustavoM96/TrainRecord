using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainRecord.Infrastructure.Interceptions;
using TrainRecord.Infrastructure.Interfaces;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Services;

namespace TrainRecord.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastuctureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        var conn = configuration.GetSection("ConnectionStrings").GetSection("DbSqlite").Value;
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(conn);
        });

        return services;
    }
};
