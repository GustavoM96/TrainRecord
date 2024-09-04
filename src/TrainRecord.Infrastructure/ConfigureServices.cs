using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainRecord.Application.Interfaces.Repositories;
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
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())
        );
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IUserActivityRepository, UserActivityRepository>();
        services.AddScoped<ITeacherStudentRepository, TeacherStudentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        var conn = configuration.GetValue<string>("ConnectionStrings:MySqlDb")!;
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySQL(conn);
        });

        return services;
    }
};
