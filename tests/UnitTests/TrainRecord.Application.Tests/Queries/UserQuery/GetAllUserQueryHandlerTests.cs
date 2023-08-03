using Moq;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class GetAllUserQueryHandlerTests : TesterBase
{
    private readonly GetAllUserQueryHandler _testClass;
    private readonly GetAllUserQuery _query;
    private readonly Mock<IUserRepository> _userRepository = new();

    public GetAllUserQueryHandlerTests()
    {
        _testClass = new GetAllUserQueryHandler(_userRepository.Object);
        _query = new GetAllUserQuery() { Pagination = PaginationOne };
    }

    [Fact]
    public async Task Test_Handle_ShouldReturnItems()
    {
        //arrange
        var users = new List<User>() { new() { Id = GuidUnique } }.AsQueryable();

        _userRepository.Setup(m => m.AsNoTracking()).Returns(users);

        //act
        var result = await _testClass.Handle(_query, default);

        //assert
        Assert.Equal(users.First().Id, result.Value.Items.First().Id);
        Assert.IsType<Page<RegisterUserResponse>>(result.Value);
    }

    [Fact]
    public async Task Test_Handle_WhenFilter_ShouldReturnFilteredItem()
    {
        //arrange
        var users = new List<User>()
        {
            new() { Email = "gustavo.hmessias96@gmail.com" },
            new() { Email = "gustavo.hmessias96@hotmail.com" },
            new() { Email = "gustavo@gmail.com" },
            new() { Email = "gustavo.hmessias96@outlook.com" }
        };

        var query = users.AsQueryable();

        var filterByGmailQuery = new GetAllUserQuery()
        {
            Pagination = new() { PageNumber = 1, PerPage = 10 },
            UserQueryRequest = new() { Email = "gmail" }
        };

        _userRepository.Setup(m => m.AsNoTracking()).Returns(query);

        //act
        var result = await _testClass.Handle(filterByGmailQuery, default);

        //assert
        var emailsResult = result.Value.Items.Select(item => item.Email);
        var gmails = new List<string>() { users[2].Email, users[0].Email };

        Assert.True(SequenceEqual(emailsResult, gmails));
        Assert.IsType<Page<RegisterUserResponse>>(result.Value);
    }
}
