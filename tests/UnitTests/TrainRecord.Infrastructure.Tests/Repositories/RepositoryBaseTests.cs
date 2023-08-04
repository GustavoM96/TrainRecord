using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Repositories;
using TrainRecord.Infrastructure.Tests.Common;

namespace TrainRecord.Infrastructure.Tests;

public class RepositoryBaseTests : InfrastructureTesterBase
{
    private static Guid[] Guids =>
        new Guid[3]
        {
            new Guid("00000000-0000-0000-0000-000000000001"),
            new Guid("00000000-0000-0000-0000-000000000002"),
            new Guid("00000000-0000-0000-0000-000000000003")
        };

    private readonly User[] _users = new User[3]
    {
        new()
        {
            Id = Guids[0],
            FirstName = "gustavo",
            LastName = "Hanrique",
            Email = "gustavomessias@gmail.com",
            Password = "Adm123"
        },
        new()
        {
            Id = Guids[1],
            FirstName = "gustavo",
            LastName = "messias",
            Email = "gustavomessias@outlook.com",
            Password = "Adm123"
        },
        new()
        {
            Id = Guids[2],
            FirstName = "caio",
            LastName = "costa",
            Email = "gustavomessias@hotmail.com",
            Password = "Adm123"
        }
    };

    private readonly IRepositoryBase<User> _testClass;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _appDbContext;

    public RepositoryBaseTests()
    {
        _appDbContext = CreateAppDbContext();
        _testClass = new UserRepository(_appDbContext);
        _unitOfWork = new UnitOfWork(_appDbContext);
        SeedDb(_users);
    }

    private void SeedDb(User[] users)
    {
        if (!_testClass.AsNoTracking().Any())
        {
            foreach (var user in users)
            {
                _testClass.AddAsync(user).Wait();
            }
            _unitOfWork.SaveChangesAsync().Wait();
        }
    }

    [Fact]
    public async Task Test_GetOtherDbSetAsync_WhenValidId_ShouldFindAny()
    {
        //arrange

        //act
        var result = await _testClass.AnyByIdAsync(new(Guids[0]));

        //assert
        Assert.IsType<bool>(result);
        Assert.True(result);
    }

    [Fact]
    public async Task Test_GetOtherDbSetAsync_WhenInValidId_ShouldNotFindAny()
    {
        //arrange

        //act
        var guidNotFound = GuidUnique;
        var result = await _testClass.AnyByIdAsync(new(guidNotFound));

        //assert
        Assert.IsType<bool>(result);
        Assert.False(result);
    }

    [Fact]
    public void Test_AsNoTracking_ShouldDetached()
    {
        //arrange

        //act
        var result = _testClass.AsNoTracking();

        //assert
        var state = _appDbContext.Entry(result.First()).State;
        Assert.Equal(3, result.Count());
        Assert.Equal(EntityState.Detached, state);
    }
}
