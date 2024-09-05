using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Repositories;
using TrainRecord.Infrastructure.Tests.Common;

namespace TrainRecord.Infrastructure.Tests;

public class UnitOfWorkTests : InfrastructureTesterBase
{
    private static Guid[] Guids =>
        [new("00000000-0000-0000-0000-000000000011"), new("00000000-0000-0000-0000-000000000012")];

    private User[] Users =>
        [
            CreateUser(Guids[0], "gustavomessias@gmail.com"),
            CreateUser(Guids[1], "gustavomessias@outlook.com"),
        ];

    private readonly IRepositoryBase<User> _repositoryBase;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _appDbContext;

    public UnitOfWorkTests()
    {
        _appDbContext = CreateAppDbContext();
        _repositoryBase = new UserRepository(_appDbContext);
        _unitOfWork = new UnitOfWork(_appDbContext);

        DeleteUserIfAny(Users[0].EntityId);
        DeleteUserIfAny(Users[1].EntityId);
    }

    [Fact]
    public async Task Test_SaveChangeAsync()
    {
        //arrange
        await _repositoryBase.AddAsync(Users[0]);

        //act
        await _unitOfWork.SaveChangesAsync();

        //assert
        Assert.True(await _repositoryBase.AnyByIdAsync(Users[0].EntityId));
    }

    [Fact]
    public async Task Test_RollBack()
    {
        //arrange
        await _repositoryBase.AddAsync(Users[0]);
        await _repositoryBase.AddAsync(Users[1]);

        //act
        var result = _unitOfWork.RollBack();

        //assert
        var user1State = _appDbContext.Entry(Users[0]).State;
        var user2State = _appDbContext.Entry(Users[1]).State;
        Assert.Equal(EntityState.Detached, user1State);
        Assert.Equal(EntityState.Detached, user2State);

        Assert.False(await _repositoryBase.AnyByIdAsync(Users[0].EntityId));
        Assert.False(await _repositoryBase.AnyByIdAsync(Users[1].EntityId));

        Assert.Equal(2, result);
    }

    private void DeleteUserIfAny(EntityId<User> entityBase)
    {
        if (_repositoryBase.AnyByIdAsync(entityBase).GetAwaiter().GetResult())
        {
            _repositoryBase.DeleteById(entityBase).Wait();
        }
    }
}
