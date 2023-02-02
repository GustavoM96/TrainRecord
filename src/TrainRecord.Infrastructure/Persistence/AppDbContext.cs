using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Persistence.Interceptions;

namespace TrainRecord.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public DbSet<UserActivity> UserActivities => Set<UserActivity>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<TeacherStudent> TeacherStudent => Set<TeacherStudent>();

    public AppDbContext(
        DbContextOptions<AppDbContext> dbContextOptions,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
    ) : base(dbContextOptions)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}
