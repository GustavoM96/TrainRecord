using Moq;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class CreateActivityCommandHandlerTests : TesterBase
{
    private readonly CreateActivityCommandHandler _testClass;
    private readonly Mock<IActivityRepository> _activityRepository = new();

    public CreateActivityCommandHandlerTests()
    {
        _testClass = new CreateActivityCommandHandler(_activityRepository.Object);
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundActivity_ShouldAddActivity()
    {
        //arrange
        var command = new CreateActivityCommand() { Name = "pular corda", };
        _activityRepository.Setup(m => m.AnyByNameAsync("pular corda")).ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(command, default);

        //assert
        _activityRepository.Verify(m => m.AddAsync(It.IsAny<Activity>()));
        Assert.IsType<Activity>(result.Value);
    }

    [Fact]
    public async Task Test_Handle_WhenFoundActivity_ShouldReturnActivityError()
    {
        //arranges
        var command = new CreateActivityCommand() { Name = "pular corda", };
        _activityRepository.Setup(m => m.AnyByNameAsync("pular corda")).ReturnsAsync(true);

        //act
        var result = await _testClass.Handle(command, default);

        //assert
        _activityRepository.Verify(m => m.AddAsync(It.IsAny<Activity>()), Times.Never);
        Assert.Equal(result.FirstError, ActivityErrors.NameAlreadyExists);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Test_CreateActivityCommandValidator(string activityName)
    {
        //arrange
        var command = new CreateActivityCommand() { Name = activityName };
        var validator = new CreateActivityCommandValidator();

        //assert
        Assert.True(await IsInvalidPropertiesAsync(validator, command, "Name"));
    }
}
