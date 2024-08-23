using Moq;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.Tests;

public class CreateActivityCommandHandlerTests : ApplicationTesterBase
{
    private readonly CreateActivityCommandHandler _testClass;
    private readonly CreateActivityCommand _command;
    private readonly Mock<IActivityRepository> _activityRepository;

    public CreateActivityCommandHandlerTests()
    {
        _activityRepository = FreezeFixture<Mock<IActivityRepository>>();

        _testClass = CreateFixture<CreateActivityCommandHandler>();
        _command = CreateFixture<CreateActivityCommand>();
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundActivity_ShouldAddActivity()
    {
        //arrange
        _activityRepository.Setup(m => m.AnyByNameAsync(_command.Name)).ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _activityRepository.Verify(m => m.AddAsync(It.IsAny<Activity>()));
        Assert.IsType<Activity>(result.Value);
    }

    [Fact]
    public async Task Test_Handle_WhenFoundActivity_ShouldReturnActivityError()
    {
        //arranges
        _activityRepository.Setup(m => m.AnyByNameAsync(_command.Name)).ReturnsAsync(true);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _activityRepository.Verify(m => m.AddAsync(It.IsAny<Activity>()), Times.Never);
        Assert.Equal(result.FirstError, ActivityErrors.NameAlreadyExists);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Test_CreateActivityCommandValidator_Error(string activityName)
    {
        //arrange
        var command = new CreateActivityCommand(activityName);
        var validator = new CreateActivityCommandValidator();

        //assert
        Assert.True(await IsInvalidPropertiesAsync(validator, command, "Name"));
    }

    [Theory]
    [InlineData("pular corda")]
    [InlineData("supino")]
    public async Task Test_CreateActivityCommandValidator_Valid(string activityName)
    {
        //arrange
        var command = new CreateActivityCommand(activityName);
        var validator = new CreateActivityCommandValidator();

        //assert
        Assert.True(await IsValidPropertiesAsync(validator, command));
    }
}
