using Moq;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class CreateUserActivityCommandHandlerTests : ApplicationTesterBase
{
    private readonly CreateUserActivityCommandHandler _testClass;
    private readonly CreateUserActivityCommand _command;
    private readonly Mock<IActivityRepository> _activityRepository;
    private readonly Mock<IUserActivityRepository> _userActivityRepository;
    private readonly Mock<IUserRepository> _userRepository;

    public CreateUserActivityCommandHandlerTests()
    {
        _activityRepository = FreezeFixture<Mock<IActivityRepository>>();
        _userActivityRepository = FreezeFixture<Mock<IUserActivityRepository>>();
        _userRepository = FreezeFixture<Mock<IUserRepository>>();

        _testClass = CreateFixture<CreateUserActivityCommandHandler>();
        _command = CreateFixture<CreateUserActivityCommand>();
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundActivity_ShouldReturnErrors()
    {
        //arrange
        _activityRepository.Setup(m => m.AnyByIdAsync(_command.ActivityId)).ReturnsAsync(false);
        _userRepository.Setup(m => m.AnyByIdAsync(_command.UserId)).ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _userActivityRepository.Verify(m => m.AddAsync(It.IsAny<UserActivity>()), Times.Never);
        Assert.Contains(UserError.NotFound, result.Errors);
        Assert.Contains(ActivityErrors.NotFound, result.Errors);
    }

    [Fact]
    public async Task Test_Handle_WhenFoundActivityAndUser_ShouldAddUserActivity()
    {
        //arrange
        _activityRepository.Setup(m => m.AnyByIdAsync(_command.ActivityId)).ReturnsAsync(true);
        _userRepository.Setup(m => m.AnyByIdAsync(_command.UserId)).ReturnsAsync(true);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _userActivityRepository.Verify(m => m.AddAsync(It.IsAny<UserActivity>()));
        Assert.IsType<UserActivity>(result.Value);
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(-354, -23, -54)]
    public async Task Test_CreateActivityCommandValidator_Error(
        int weight,
        int repetition,
        int serie
    )
    {
        //arrange
        var command = new CreateUserActivityCommand()
        {
            UserId = new(GuidUnique),
            ActivityId = new(GuidUnique),
            Weight = weight,
            Repetition = repetition,
            Serie = serie
        };
        var validator = new CreateUserActivityCommandValidator();

        //assert
        Assert.True(
            await IsInvalidPropertiesAsync(
                validator,
                command,
                new string[3] { "Weight", "Repetition", "Serie" }
            )
        );
    }

    [Theory]
    [InlineData(0, 1, 1)]
    [InlineData(43, 10, 21)]
    public async Task Test_CreateActivityCommandValidator_Valid(
        int weight,
        int repetition,
        int serie
    )
    {
        //arrange
        var command = new CreateUserActivityCommand()
        {
            UserId = new(GuidUnique),
            ActivityId = new(GuidUnique),
            Weight = weight,
            Repetition = repetition,
            Serie = serie
        };
        var validator = new CreateUserActivityCommandValidator();

        //assert
        Assert.True(await IsValidPropertiesAsync(validator, command));
    }
}
