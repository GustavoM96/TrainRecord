using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;
using TrainRecord.Core.Services.Auth;
using TrainRecord.Infrastructure.Persistence;

namespace WebApp.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private AppDbContext _appDbContext = default!;
    private ITokenGenerator _tokenGenerator = default!;
    public ApiTokenResponse TokenAdm { get; set; } = default!;
    public ApiTokenResponse TokenUser { get; set; } = default!;
    public User Adm = CreateUser("gustavoTestAdm@gmail.com", Role.Adm);
    public User User = CreateUser("gustavoTestUser@gmail.com", Role.User);

    private readonly MsSqlContainer _mySqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseEnvironment("Test")
            .ConfigureTestServices(services =>
            {
                var type = typeof(DbContextOptions<AppDbContext>);
                var descriptor = services.First(des => des.ServiceType == type);
                services.Remove(descriptor);

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(_mySqlContainer.GetConnectionString())
                );

                using var scope = services.BuildServiceProvider().CreateScope();
                _appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _tokenGenerator = scope.ServiceProvider.GetRequiredService<ITokenGenerator>();

                _appDbContext.Database.EnsureCreated();
                TokenAdm = AddUserToDatabase(Adm).Result;
                TokenUser = AddUserToDatabase(User).Result;
            });
    }

    public static User CreateUser(string email, Role role) =>
        new()
        {
            Email = email,
            Password = "#Teste1234",
            FirstName = "Gustavo",
            LastName = "Messias",
            Role = role,
        };

    public async Task<ApiTokenResponse> AddUserToDatabase(User user)
    {
        _appDbContext.Users.Add(user);
        await _appDbContext.SaveChangesAsync();
        return _tokenGenerator.Generate(user);
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
    }
}
