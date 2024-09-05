using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Persistence.Interceptions;

namespace TrainRecord.Infrastructure.Tests.Common;

public abstract class InfrastructureTesterBase
{
    protected static AppDbContext CreateAppDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("InMemoryDatabaseForTests")
            .Options;

        AuditableEntitySaveChangesInterceptor? interceptor = null!;
        var appDbContext = new AppDbContext(options, interceptor);
        appDbContext.Database.EnsureCreated();

        return appDbContext;
    }

    protected static Guid GuidUnique => Guid.NewGuid();

    protected static User CreateUser(Guid id, string email) =>
        new()
        {
            Id = id,
            FirstName = "Gustavo",
            LastName = "Henrique",
            Email = email,
            Password = "Adm123",
        };
}
