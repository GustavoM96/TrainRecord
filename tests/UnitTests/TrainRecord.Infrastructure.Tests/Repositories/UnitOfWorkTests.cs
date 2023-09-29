using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Infrastructure.Persistence;
using TrainRecord.Infrastructure.Repositories;
using TrainRecord.Infrastructure.Tests.Common;

namespace TrainRecord.Infrastructure.Tests;

public class UnitOfWorkTests : InfrastructureTesterBase
{
    private static Guid[] Guids =>
        new Guid[4]
        {
            new Guid("00000000-0000-0000-0000-000000000011"),
            new Guid("00000000-0000-0000-0000-000000000012"),
            new Guid("00000000-0000-0000-0000-000000000013"),
            new Guid("00000000-0000-0000-0000-000000000014")
        };

    private readonly User[] _users = new User[2]
    {
        new()
        {
            Id = Guids[0],
            FirstName = "Gustavo",
            LastName = "Tests",
            Email = "gustavo.teste@gmail.com",
            Password = "Adm123"
        },
        new()
        {
            Id = Guids[1],
            FirstName = "Gustavo2",
            LastName = "Tests2",
            Email = "gustavo2.teste@gmail.com",
            Password = "Adm123"
        }
    };

    private readonly IRepositoryBase<User> _repositoryBase;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _appDbContext;

    public UnitOfWorkTests()
    {
        _appDbContext = CreateAppDbContext();
        _repositoryBase = new UserRepository(_appDbContext);
        _unitOfWork = new UnitOfWork(_appDbContext);

        DeleteUserIfAny(_users[0].EntityId);
        DeleteUserIfAny(_users[1].EntityId);
    }

    [Fact]
    public async Task Test_SaveChangeAsync()
    {
        //arrange
        await _repositoryBase.AddAsync(_users[0]);

        //act
        _unitOfWork.SaveChangesAsync().Wait();

        //assert
        Assert.True(await _repositoryBase.AnyByIdAsync(_users[0].EntityId));
    }

    [Fact]
    public async Task Test_RollBack()
    {
        //arrange
        await _repositoryBase.AddAsync(_users[0]);
        await _repositoryBase.AddAsync(_users[1]);

        //act
        var result = _unitOfWork.RollBack();

        //assert
        var user1State = _appDbContext.Entry(_users[0]).State;
        var user2State = _appDbContext.Entry(_users[1]).State;
        Assert.Equal(EntityState.Detached, user1State);
        Assert.Equal(EntityState.Detached, user2State);

        Assert.False(await _repositoryBase.AnyByIdAsync(_users[0].EntityId));
        Assert.False(await _repositoryBase.AnyByIdAsync(_users[1].EntityId));

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
