using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Repositories;
using TrainRecord.Infrastructure.Tests.Common;

namespace TrainRecord.Infrastructure.Tests;

public class RepositoryBaseTests : InfrastructureTesterBase
{
    private static Guid[] Guids =>
        [
            new Guid("00000000-0000-0000-0000-000000000001"),
            new Guid("00000000-0000-0000-0000-000000000002"),
            new Guid("00000000-0000-0000-0000-000000000003"),
            new Guid("00000000-0000-0000-0000-000000000004"),
        ];

    private static User[] Users =>
        [
            CreateUser(Guids[0], "gustavomessias@gmail.com"),
            CreateUser(Guids[1], "gustavomessias@outlook.com"),
            CreateUser(Guids[2], "gustavomessias@hotmail.com"),
        ];

    private readonly IRepositoryBase<User> _testClass;
    private readonly AppDbContext _appDbContext;

    public RepositoryBaseTests()
    {
        _appDbContext = CreateAppDbContext();
        _testClass = new UserRepository(_appDbContext);
        SeedDb(Users);
    }

    private void SeedDb(User[] users)
    {
        if (!_testClass.AsNoTracking().Any())
        {
            foreach (var user in users)
            {
                _testClass.AddAsync(user).Wait();
            }
            _appDbContext.SaveChangesAsync().Wait();
        }
    }

    [Fact]
    public async Task Test_FindByIdAsync_WhenValidId_ShouldFindAny()
    {
        //arrange

        //act
        var result = await _testClass.FindByIdAsync(new(Guids[0]));

        //assert
        Assert.IsType<User>(result);
        Assert.Equal(Users[0].Id, result.Id);
    }

    [Fact]
    public async Task Test_FindByIdAsync_WhenInValidId_ShouldNotFindAny()
    {
        //arrange

        //act
        var guidNotFound = GuidUnique;
        var result = await _testClass.FindByIdAsync(new(guidNotFound));

        //assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Test_AnyByIdAsync_WhenValidId_ShouldFindAny()
    {
        //arrange

        //act
        var result = await _testClass.AnyByIdAsync(new(Guids[0]));

        //assert
        Assert.IsType<bool>(result);
        Assert.True(result);
    }

    [Fact]
    public async Task Test_AnyByIdAsync_WhenInValidId_ShouldNotFindAny()
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
        Assert.True(result.Count() >= 3);
        Assert.Equal(EntityState.Detached, state);
    }

    [Fact]
    public void Test_Page()
    {
        //arrange
        var pagination = new Pagination() { PageNumber = 1, PerPage = 2 };
        //act
        var result = _testClass.AsPage(pagination);

        //assert
        var state = _appDbContext.Entry(result.Items.First()).State;
        Assert.Equal(EntityState.Detached, state);

        Assert.Contains(result.Items, item => item.Id == Guids[0]);
        Assert.Contains(result.Items, item => item.Id == Guids[1]);

        Assert.Equal(2, result.Items.Count());
        Assert.IsType<Page<User>>(result);
    }

    [Fact]
    public void Test_PageAdapted()
    {
        //arrange
        var pagination = new Pagination() { PageNumber = 1, PerPage = 2 };
        //act
        var result = _testClass.AsPage<RegisterUserResponse>(pagination);

        //assert
        Assert.Contains(result.Items, item => item.Id == Guids[0]);
        Assert.Contains(result.Items, item => item.Id == Guids[1]);

        Assert.Equal(2, result.Items.Count());
        Assert.IsType<Page<RegisterUserResponse>>(result);
    }

    [Fact]
    public async Task Test_AddAsync_ShouldChangeStateAdded()
    {
        //arrange
        var user = CreateUser(Guids[3], "gustavomessias@net.com");

        //act
        await _testClass.AddAsync(user);

        //assert
        var state = _appDbContext.Entry(user).State;
        Assert.Equal(EntityState.Added, state);
    }

    [Fact]
    public void Test_Update_ShouldChangeStateModified()
    {
        //arrange
        var user = CreateUser(Guids[3], "gustavomessias@net.com");

        //act
        _testClass.Update(user);

        //assert
        var state = _appDbContext.Entry(user).State;
        Assert.Equal(EntityState.Modified, state);
    }

    [Fact]
    public async Task Test_Delete_WhenFoundUser_ShouldReturnTrue()
    {
        //arrange
        var user = CreateUser(Guids[3], "gustavomessias@net.com");

        if (!await _testClass.AnyByIdAsync(user.EntityId))
        {
            await _testClass.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
        }

        //act
        _testClass.Remove(user);
        await _appDbContext.SaveChangesAsync();

        //assert
        Assert.False(await _testClass.AnyByIdAsync(user.EntityId));
    }
}
