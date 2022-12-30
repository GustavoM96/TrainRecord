using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<UserActivity> UserActivities => Set<UserActivity>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Activity> Activities => Set<Activity>();

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
