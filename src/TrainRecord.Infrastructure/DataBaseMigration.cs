using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure;

public static class DataBaseMigration
{
    public static async Task Migrate(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }
};
