using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastuctureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var conn = configuration.GetSection("ConnectionStrings").GetSection("DbSqlite").Value;
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(conn);
        });


        return services;
    }
};
