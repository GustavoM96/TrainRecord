using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using TrainRecord.Infrastructure.Persistence;

namespace WebApp.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private AppDbContext _appDbContext = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Test")
            .ConfigureServices(service =>
            {
                using var scope = service.BuildServiceProvider().CreateScope();
                _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _appDbContext.Database.EnsureDeleted();
                _appDbContext.Database.EnsureCreated();
            });
    }

    protected void DeleteUsers() { }
}
