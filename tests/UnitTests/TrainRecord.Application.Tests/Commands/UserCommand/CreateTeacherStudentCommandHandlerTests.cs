using Moq;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Application.UserCommand;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;

namespace TrainRecord.Application.Tests;

public class CreateTeacherStudentCommandHandlerTests : ApplicationTesterBase
{
    private readonly CreateTeacherStudentCommandHandler _testClass;
    private readonly CreateTeacherStudentCommand _command;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<ITeacherStudentRepository> _teacherStudentRepository;

    public CreateTeacherStudentCommandHandlerTests()
    {
        _userRepository = FreezeFixture<Mock<IUserRepository>>();
        _teacherStudentRepository = FreezeFixture<Mock<ITeacherStudentRepository>>();

        _testClass = CreateFixture<CreateTeacherStudentCommandHandler>();
        _command = CreateFixture<CreateTeacherStudentCommand>();
    }

    [Fact]
    public async Task Test_Handle_WhenFoundTeacherAndStudent_ShouldAddTeacherAndStudent()
    {
        //arrange
        var teacher = new User() { Role = Role.Teacher };

        _userRepository.Setup(m => m.AnyByIdAsync(_command.StudentId)).ReturnsAsync(true);
        _userRepository.Setup(m => m.FindByIdAsync(_command.TeacherId)).ReturnsAsync(teacher);
        _teacherStudentRepository
            .Setup(m => m.GetByTeacherStudentId(_command.StudentId, _command.TeacherId))
            .ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _teacherStudentRepository.Verify(m =>
            m.AddAsync(It.Is<TeacherStudent>(ts => ts.TeacherId == _command.TeacherId))
        );
        _teacherStudentRepository.Verify(m =>
            m.AddAsync(It.Is<TeacherStudent>(ts => ts.StudentId == _command.StudentId))
        );
        Assert.IsType<TeacherStudent>(result.Value);
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundTeacherAndStudent_ShouldReturnErrors()
    {
        //arrange
        User? teacher = null;

        _userRepository.Setup(m => m.AnyByIdAsync(_command.StudentId)).ReturnsAsync(false);
        _userRepository.Setup(m => m.FindByIdAsync(_command.TeacherId)).ReturnsAsync(teacher);
        _teacherStudentRepository
            .Setup(m => m.GetByTeacherStudentId(_command.StudentId, _command.TeacherId))
            .ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _teacherStudentRepository.Verify(m => m.AddAsync(It.IsAny<TeacherStudent>()), Times.Never);
        Assert.Contains(UserError.StudentNotFound, result.Errors);
        Assert.Contains(UserError.TeacherNotFound, result.Errors);
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundTeacherWithNotRole_ShouldReturnErrors()
    {
        //arrange
        var teacherWithNoRole = new User() { Role = Role.User };

        _userRepository.Setup(m => m.AnyByIdAsync(_command.StudentId)).ReturnsAsync(true);
        _userRepository
            .Setup(m => m.FindByIdAsync(_command.TeacherId))
            .ReturnsAsync(teacherWithNoRole);

        _teacherStudentRepository
            .Setup(m => m.GetByTeacherStudentId(_command.StudentId, _command.TeacherId))
            .ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _teacherStudentRepository.Verify(m => m.AddAsync(It.IsAny<TeacherStudent>()), Times.Never);
        Assert.Contains(UserError.IsNotTeacher, result.Errors);
    }

    [Fact]
    public async Task Test_Handle_WhenAlreadyRegistered_ShouldReturnErrors()
    {
        var teacher = new User() { Role = Role.Teacher };

        _userRepository.Setup(m => m.AnyByIdAsync(_command.StudentId)).ReturnsAsync(true);
        _userRepository.Setup(m => m.FindByIdAsync(_command.TeacherId)).ReturnsAsync(teacher);
        _teacherStudentRepository
            .Setup(m => m.GetByTeacherStudentId(_command.StudentId, _command.TeacherId))
            .ReturnsAsync(true);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _teacherStudentRepository.Verify(m => m.AddAsync(It.IsAny<TeacherStudent>()), Times.Never);
        Assert.Contains(UserError.TeacherStudentExists, result.Errors);
    }
}
