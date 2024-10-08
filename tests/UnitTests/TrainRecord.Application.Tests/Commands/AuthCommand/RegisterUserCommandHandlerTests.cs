using Moq;
using TrainRecord.Application.AuthCommand;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Application.Responses;
using TrainRecord.Application.Tests.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.Tests;

public class RegisterUserCommandHandlerTests : ApplicationTesterBase
{
    private readonly RegisterUserCommandHandler _testClass;
    private readonly RegisterUserCommand _command;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IHashGenerator> _hashGenerator;

    public RegisterUserCommandHandlerTests()
    {
        _userRepository = FreezeFixture<Mock<IUserRepository>>();
        _hashGenerator = FreezeFixture<Mock<IHashGenerator>>();

        _testClass = CreateFixture<RegisterUserCommandHandler>();
        _command = CreateFixture<RegisterUserCommand>();
    }

    [Fact]
    public async Task Test_Handle_WhenNotFoundEmail_ShouldAddUser()
    {
        //arrange
        var hashedPassword = "!wds043mdfaAdDSeSAw435r";
        _userRepository.Setup(m => m.AnyByEmailAsync(_command.Email)).ReturnsAsync(false);
        _hashGenerator.Setup(m => m.Generate(It.IsAny<string>())).Returns(hashedPassword);

        //act
        var result = await _testClass.Handle(_command, default);

        //assert
        _userRepository.Verify(m => m.AddAsync(It.Is<User>(user => user.Email == _command.Email)));
        _userRepository.Verify(m =>
            m.AddAsync(It.Is<User>(user => user.Password == hashedPassword))
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
        _hashGenerator.Verify(m => m.Generate(_command.Password));
        _hashGenerator.Verify(m => m.Generate(_command.Password));
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
        var commandAdm = new RegisterUserCommand(
            "gustavoAdm@gmail.com",
            "Gus#123Adm",
            "gustavoAdm",
            "messiasAdm",
            Role.Adm
        );

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
    public async Task Test_RegisterUserCommandValidator_Error(string? email, string? password)
    {
        //arrange
        var command = new RegisterUserCommand(email, password, "gustavo", "messias", Role.User);

        var validator = new RegisterUserCommandValidator();

        //assert
        AssertExtensions.AreInvalidProperties(validator, command, "Email", "Password");
    }

    [Theory]
    [InlineData("gustavo@gmail.com", "abc23F@$@#deF")]
    [InlineData("gustavo@hotmail.com", "!@WEd4")]
    public async Task Test_RegisterUserCommandValidator_Success(string email, string password)
    {
        //arrange
        var command = new RegisterUserCommand(email, password, "gustavo", "messias", Role.User);

        var validator = new RegisterUserCommandValidator();

        //assert
        AssertExtensions.AreValidProperties(validator, command);
    }
}
