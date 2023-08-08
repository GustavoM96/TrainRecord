using Moq;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class GetUserByIdQueryHandlerTests : ApplicationTesterBase
{
    private readonly GetUserByIdQueryHandler _testClass;
    private readonly GetUserByIdQuery _query;
    private readonly Mock<IUserRepository> _userRepository;

    public GetUserByIdQueryHandlerTests()
    {
        _userRepository = FreezeFixture<Mock<IUserRepository>>();

        _testClass = CreateFixture<GetUserByIdQueryHandler>();
        _query = CreateFixture<GetUserByIdQuery>();
    }

    [Fact]
    public async Task Test_Handle_WhenFoundUser_ShouldReturnItem()
    {
        //arrange
        var user = new User() { FirstName = "gustavo" };
        _userRepository.Setup(m => m.FindByIdAsync(_query.UserId)).ReturnsAsync(user);

        //act
        var result = await _testClass.Handle(_query, default);

        //assert
        Assert.Equal(user.FirstName, result.Value.FirstName);
        Assert.IsType<RegisterUserResponse>(result.Value);
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundUser_ShouldReturnErros()
    {
        //arrange
        User? user = null;
        _userRepository.Setup(m => m.FindByIdAsync(_query.UserId)).ReturnsAsync(user);

        //act
        var result = await _testClass.Handle(_query, default);

        //assert
        Assert.Contains(UserError.NotFound, result.Errors);
    }
}
