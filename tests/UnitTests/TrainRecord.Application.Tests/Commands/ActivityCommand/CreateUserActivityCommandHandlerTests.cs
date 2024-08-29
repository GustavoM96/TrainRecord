using Moq;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.Tests;

public class CreateUserActivityCommandHandlerTests : ApplicationTesterBase
{
    private readonly CreateUserActivityCommandHandler _testClass;
    private readonly CreateUserActivityCommand _command;
    private readonly Mock<IActivityRepository> _activityRepository;
    private readonly Mock<IUserActivityRepository> _userActivityRepository;
    private readonly Mock<ITeacherStudentRepository> _teacherStudentRepository;
    private readonly Mock<IUserRepository> _userRepository;

    public CreateUserActivityCommandHandlerTests()
    {
        _activityRepository = FreezeFixture<Mock<IActivityRepository>>();
        _userActivityRepository = FreezeFixture<Mock<IUserActivityRepository>>();
        _userRepository = FreezeFixture<Mock<IUserRepository>>();
        _teacherStudentRepository = FreezeFixture<Mock<ITeacherStudentRepository>>();

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
        _teacherStudentRepository
            .Setup(m => m.IsTeacherStudent(_command.UserId, _command.TeacherId!))
            .ReturnsAsync(true);

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
        var command = new CreateUserActivityCommand(
            new(GuidUnique),
            new(GuidUnique),
            new(GuidUnique),
            weight,
            repetition,
            serie,
            "A",
            "My top trainingName",
            new TimeOnly(0, 1)
        );
        var validator = new CreateUserActivityCommandValidator();

        //assert
        AssertExtensions.AreInvalidProperties(
            validator,
            command,
            ["Weight", "Repetition", "Serie"]
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
        var command = new CreateUserActivityCommand(
            new(GuidUnique),
            new(GuidUnique),
            new(GuidUnique),
            weight,
            repetition,
            serie,
            "A",
            "My top trainingName",
            new TimeOnly(0, 1)
        );
        var validator = new CreateUserActivityCommandValidator();

        //assert
        AssertExtensions.AreValidProperties(validator, command);
    }
}
