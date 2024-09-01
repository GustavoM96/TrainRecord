using System.Net;
using System.Net.Http.Json;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Requests;
using TrainRecord.Core.Enum;

namespace WebApp.Tests.Auth;

public class RegisterTests : CustomClassFixture
{
    private const string RegisterUri = "api/Auth/Register";
    private const string LoginUri = "api/Auth/Login";
    private const string Password = "#Teste1234";

    private static RegisterUserRequest CreateRegisterRequest(string email) =>
        new(email, Password, "Gustavo", "Messias", Role.User);

    private static LoginUserRequest CreateLoginRequest(string email) => new(email, Password);

    public RegisterTests(CustomWebApplicationFactory customWebApplicationFactory)
        : base(customWebApplicationFactory) { }

    [Fact]
    public async Task Register_Success()
    {
        //arrange
        var request = CreateRegisterRequest("gustavo@gmail.com");

        //act
        var result = await _httpClient.PostAsJsonAsync(RegisterUri, request);

        //assert
        var data = await GetDataPropertyAsync(result);

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal(request.Email, data.GetProperty("email").ToString());
        Assert.Equal(request.FirstName, data.GetProperty("firstName").ToString());
        Assert.Equal(request.LastName, data.GetProperty("lastName").ToString());
        Assert.Equal(request.Role.ToString(), data.GetProperty("role").ToString());
        Assert.False(data.TryGetProperty("password", out var _));

        //Login test
        var requestLogin = CreateLoginRequest(request.Email);
        var loginResult = await _httpClient.PostAsJsonAsync(LoginUri, requestLogin);

        Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
    }

    [Fact]
    public async Task Register_When_InvalidRequest_ShouldReturn_Status400AndErrors()
    {
        //arrange
        var request = CreateRegisterRequest("invalidEmail");

        //act
        var result = await _httpClient.PostAsJsonAsync(RegisterUri, request);

        //assert
        var error = await GetSingleErrorPropertyAsync(result);
        var desciption = error.GetProperty("description").ToString();

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("'Email' é um endereço de email inválido.", desciption);

        //Login test
        var requestLogin = CreateLoginRequest(request.Email);
        var loginResult = await _httpClient.PostAsJsonAsync(LoginUri, requestLogin);
        Assert.Equal(HttpStatusCode.BadRequest, loginResult.StatusCode);
    }

    [Fact]
    public async Task Login_Success()
    {
        //arrange
        var request = CreateRegisterRequest("gustavo1@gmail.com");
        await _httpClient.PostAsJsonAsync(RegisterUri, request);

        var requestLogin = CreateLoginRequest(request.Email);

        //act
        var result = await _httpClient.PostAsJsonAsync(LoginUri, requestLogin);

        //assert
        var data = await GetDataPropertyAsync(result);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotEmpty(data.GetProperty("idToken").ToString());
        Assert.False(data.TryGetProperty("password", out var _));
    }

    [Fact]
    public async Task Login_WhenEmailNotRegisterd_ShouldReturn_Status404AndErros()
    {
        //arrange
        var requestLogin = CreateLoginRequest("gustavoNotRegistered@gmail.com");

        //act
        var result = await _httpClient.PostAsJsonAsync(LoginUri, requestLogin);

        //assert
        var error = await GetSingleErrorPropertyAsync(result);
        var desciption = error.GetProperty("description").ToString();

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Equal(UserError.LoginInvalid.Description, desciption);
    }
}
