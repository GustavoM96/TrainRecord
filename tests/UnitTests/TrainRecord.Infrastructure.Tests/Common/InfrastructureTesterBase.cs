using Microsoft.EntityFrameworkCore;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Persistence.Interceptions;

namespace TrainRecord.Infrastructure.Tests.Common;

public abstract class InfrastructureTesterBase
{
    protected static AppDbContext CreateAppDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=TrainRecordDB.db;")
            .Options;

        AuditableEntitySaveChangesInterceptor? interceptor = null!;
        var appDbContext = new AppDbContext(options, interceptor);
        appDbContext.Database.EnsureCreated();

        return appDbContext;
    }

    protected static Guid GuidUnique => Guid.NewGuid();
}
