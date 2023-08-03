using FluentValidation;
using Moq;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class CreateUserActivityCommandHandlerTests : TesterBase
{
    private readonly CreateUserActivityCommandHandler _testClass;
    private readonly Mock<IActivityRepository> _activityRepository = new();
    private readonly Mock<IUserActivityRepository> _userActivityRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();

    public CreateUserActivityCommandHandlerTests()
    {
        _testClass = new CreateUserActivityCommandHandler(
            _userActivityRepository.Object,
            _userRepository.Object,
            _activityRepository.Object
        );
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundActivity_ShouldReturnErrors()
    {
        //arrange
        var command = new CreateUserActivityCommand()
        {
            UserId = new(GuidUnique),
            ActivityId = new(GuidUnique),
            Weight = 10,
            Repetition = 10,
            Serie = 4
        };

        _activityRepository.Setup(m => m.AnyByIdAsync(command.ActivityId)).ReturnsAsync(false);
        _userRepository.Setup(m => m.AnyByIdAsync(command.UserId)).ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(command, default);

        //assert
        _userActivityRepository.Verify(m => m.AddAsync(It.IsAny<UserActivity>()), Times.Never);
        Assert.Contains(UserError.NotFound, result.Errors);
        Assert.Contains(ActivityErrors.NotFound, result.Errors);
    }

    [Fact]
    public async Task Test_Handle_WhenFoundActivityAndUser_ShouldAddUserActivity()
    {
        //arrange
        var command = new CreateUserActivityCommand()
        {
            UserId = new(GuidUnique),
            ActivityId = new(GuidUnique),
            Weight = 10,
            Repetition = 10,
            Serie = 4
        };

        _activityRepository.Setup(m => m.AnyByIdAsync(command.ActivityId)).ReturnsAsync(true);
        _userRepository.Setup(m => m.AnyByIdAsync(command.UserId)).ReturnsAsync(true);

        //act
        var result = await _testClass.Handle(command, default);

        //assert


        _userActivityRepository.Verify(m => m.AddAsync(It.IsAny<UserActivity>()));
        Assert.IsType<UserActivity>(result.Value);
    }

    [Fact]
    public async Task Test_CreateActivityCommandValidator()
    {
        //arrange
        var command = new CreateUserActivityCommand()
        {
            UserId = new(GuidUnique),
            ActivityId = new(GuidUnique),
            Weight = -1,
            Repetition = 0,
            Serie = 0
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
}
