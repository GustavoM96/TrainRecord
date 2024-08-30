using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TrainRecord.Application.Errors;
using TrainRecord.Application.Requests;
using TrainRecord.Core.Enum;

namespace WebApp.Tests.Auth;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private const string Method = "api/Auth/Register";

    private RegisterUserRequest CreateRequest(string email) =>
        new(email, "#Teste1234", "Gustavo", "Messias", Role.User);

    public RegisterTests(CustomWebApplicationFactory customWebApplicationFactory)
    {
        _httpClient = customWebApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Register_Success()
    {
        //arrange
        var request = CreateRequest("gustavo@gmail.com");

        //act
        var result = await _httpClient.PostAsJsonAsync(Method, request);

        //assert
        var content = await result.Content.ReadAsStringAsync();
        var body = JsonDocument.Parse(content);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal(request.Email, data.GetProperty("email").ToString());
        Assert.Equal(request.FirstName, data.GetProperty("firstName").ToString());
        Assert.Equal(request.LastName, data.GetProperty("lastName").ToString());
        Assert.Equal(request.Role.ToString(), data.GetProperty("role").ToString());
        Assert.False(data.TryGetProperty("password", out var _));

        var requestLogin = new LoginUserRequest(request.Email, request.Password);
        var loginResult = await _httpClient.PostAsJsonAsync("api/Auth/Login", requestLogin);

        Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
    }

    [Fact]
    public async Task Register_When_InvalidRequest_ShouldReturn_Status400AndErrors()
    {
        //arrange
        var request = CreateRequest("invalidEmail");

        //act
        var result = await _httpClient.PostAsJsonAsync(Method, request);

        //assert
        var content = await result.Content.ReadAsStringAsync();
        var body = JsonDocument.Parse(content);
        var error = body
            .RootElement.GetProperty("errors")
            .EnumerateArray()
            .Single()
            .GetProperty("description")
            .ToString();

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("'Email' é um endereço de email inválido.", error);
        Assert.False(body.RootElement.TryGetProperty("password", out var _));

        var requestLogin = new LoginUserRequest(request.Email, request.Password);
        var loginResult = await _httpClient.PostAsJsonAsync("api/Auth/Login", requestLogin);
        Assert.Equal(HttpStatusCode.BadRequest, loginResult.StatusCode);
    }

    [Fact]
    public async Task Login_Success()
    {
        //arrange
        var request = CreateRequest("gustavo1@gmail.com");

        await _httpClient.PostAsJsonAsync(Method, request);

        //act
        var requestLogin = new LoginUserRequest(request.Email, request.Password);
        var result = await _httpClient.PostAsJsonAsync("api/Auth/Login", requestLogin);

        //assert
        var content = await result.Content.ReadAsStringAsync();
        var body = JsonDocument.Parse(content);
        var data = body.RootElement.GetProperty("data");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.NotEmpty(data.GetProperty("idToken").ToString());
        Assert.False(data.TryGetProperty("password", out var _));
    }

    [Fact]
    public async Task Login_WhenEmailNotRegisterd_ShouldReturn_Status404AndErros()
    {
        //arrange
        var request = CreateRequest("gustavoNotRegistered@gmail.com");

        //act
        var requestLogin = new LoginUserRequest(request.Email, request.Password);
        var result = await _httpClient.PostAsJsonAsync("api/Auth/Login", requestLogin);

        //assert
        var loginContent = await result.Content.ReadAsStringAsync();

        var body = JsonDocument.Parse(loginContent);
        var errorLogin = body
            .RootElement.GetProperty("errors")
            .EnumerateArray()
            .Single()
            .GetProperty("description")
            .ToString();

        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Equal(UserError.LoginInvalid.Description, errorLogin);
    }
}
