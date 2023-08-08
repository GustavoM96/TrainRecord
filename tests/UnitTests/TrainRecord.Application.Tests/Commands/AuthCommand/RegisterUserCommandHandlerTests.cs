using Moq;
using TrainRecord.Application.AuthCommand;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;
using TrainRecord.Infrastructure.Interfaces.Repositories;

namespace TrainRecord.Application.Tests;

public class RegisterUserCommandHandlerTests : ApplicationTesterBase
{
    private readonly RegisterUserCommandHandler _testClass;
    private readonly RegisterUserCommand _command;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IGenaratorHash> _genaratorHash;

    public RegisterUserCommandHandlerTests()
    {
        _userRepository = FreezeFixture<Mock<IUserRepository>>();
        _genaratorHash = FreezeFixture<Mock<IGenaratorHash>>();

        _testClass = CreateFixture<RegisterUserCommandHandler>();
        _command = CreateFixture<RegisterUserCommand>();
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundEmail_ShouldAddUser()
    {
        //arrange
        var hashedPassword = "!wds043mdfaAdDSeSAw435r";
        _userRepository.Setup(m => m.AnyByEmailAsync(_command.Email)).ReturnsAsync(false);
        _genaratorHash.Setup(m => m.Generate(It.IsAny<User>())).Returns(hashedPassword);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _userRepository.Verify(m => m.AddAsync(It.Is<User>(user => user.Email == _command.Email)));
        _userRepository.Verify(
            m => m.AddAsync(It.Is<User>(user => user.Password == hashedPassword))
        );
        Assert.IsType<RegisterUserResponse>(result.Value);
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundEmail_ShouldGeneratePassword()
    {
        //arrange
        _userRepository.Setup(m => m.AnyByEmailAsync(_command.Email)).ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _genaratorHash.Verify(m => m.Generate(It.Is<User>(user => user.Email == _command.Email)));
        _genaratorHash.Verify(
            m => m.Generate(It.Is<User>(user => user.Password == _command.Password))
        );
        Assert.IsType<RegisterUserResponse>(result.Value);
    }

    [Fact]
    public async Task Test_Handle_WhenFoundEmail_ShouldReturnErros()
    {
        //arrange
        _userRepository.Setup(m => m.AnyByEmailAsync(_command.Email)).ReturnsAsync(true);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _userRepository.Verify(m => m.AddAsync(It.IsAny<User>()), Times.Never);
        Assert.Contains(UserError.EmailExists, result.Errors);
    }

    [Fact]
    public async Task Test_Handle_WhenRegisterAdmAndIsNotAnotherAdm_ShouldReturnErros()
    {
        //arrange
        var commandAdm = new RegisterUserCommand()
        {
            Email = "gustavoAdm@gmail.com",
            FirstName = "gustavoAdm",
            LastName = "messiasAdm",
            Password = "Gus#123Adm",
            Role = Role.Adm
        };

        _userRepository.Setup(m => m.AnyByEmailAsync(commandAdm.Email)).ReturnsAsync(false);

        //act
        var result = await _testClass.Handle(commandAdm, default);

        //assert
        _userRepository.Verify(m => m.AddAsync(It.IsAny<User>()), Times.Never);
        Assert.Contains(UserError.RegisterAdmInvalid, result.Errors);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    [InlineData("gustavo", "1234")]
    [InlineData("gustavo.com", "abcdeF")]
    [InlineData("@gustavo.com", "S1a")]
    public async Task Test_RegisterUserCommandValidator_Error(string email, string password)
    {
        //arrange
        var command = new RegisterUserCommand()
        {
            Email = email,
            FirstName = "gustavo",
            LastName = "messias",
            Password = password,
            Role = Role.User
        };

        var validator = new RegisterUserCommandValidator();

        //assert
        Assert.True(await IsInvalidPropertiesAsync(validator, command, "Email", "Password"));
    }

    [Theory]
    [InlineData("gustavo@gmail.com", "abc23F@$@#deF")]
    [InlineData("gustavo@hotmail.com", "!@WEd4")]
    public async Task Test_RegisterUserCommandValidator_Success(string email, string password)
    {
        //arrange
        var command = new RegisterUserCommand()
        {
            Email = email,
            FirstName = "gustavo",
            LastName = "messias",
            Password = password,
            Role = Role.User
        };

        var validator = new RegisterUserCommandValidator();

        //assert
        Assert.True(await IsValidPropertiesAsync(validator, command));
    }
}
