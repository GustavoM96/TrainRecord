using System.Net;
using System.Net.Http.Json;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.Errors;
using WebApp.Tests.Common;

namespace WebApp.Tests.Activity;

public class CreateActivityTests : CustomClassFixture
{
    private const string Uri = "api/Activity";

    private static CreateActivityCommand CreateRequest(string name) => new(name);

    public CreateActivityTests(CustomWebApplicationFactory customWebApplicationFactory)
        : base(customWebApplicationFactory) { }

    [Fact]
    public async Task Create_Success()
    {
        //arrange
        var request = CreateRequest("pular corda triplo");

        var token = GetAdmToken();
        AddAuthentication(token.Key);

        //act
        var result = await _httpClient.PostAsJsonAsync(Uri, request);

        //assert
        var data = await GetDataPropertyAsync(result);

        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal(request.Name, data.GetProperty("name").ToString());

        //Get Activity
        var getActivityResult = await _httpClient.GetAsync(Uri);
        var getActivityData = await GetDataPropertyAsync(result);

        Assert.Equal(HttpStatusCode.OK, getActivityResult.StatusCode);
        Assert.Equal(request.Name, getActivityData.GetProperty("name").ToString());
    }

    [Fact]
    public async Task Create_When_InvalidName_ShouldReturn_Status400AndErros()
    {
        //arrange
        var request = CreateRequest("");

        var token = GetAdmToken();
        AddAuthentication(token.Key);

        //act
        var result = await _httpClient.PostAsJsonAsync(Uri, request);

        //assert
        var error = await GetSingleErrorPropertyAsync(result);
        var desciption = error.GetProperty("description").ToString();

        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("'Name' deve ser informado.", desciption);
    }

    [Fact]
    public async Task Create_When_NotAdmToken_ShouldReturn_Status403AndErros()
    {
        //arrange
        var request = CreateRequest("pular corda duplo pulo");

        var token = GetUserToken();
        AddAuthentication(token.Key);

        //act
        var result = await _httpClient.PostAsJsonAsync(Uri, request);

        //assert
        var error = await GetSingleErrorPropertyAsync(result);
        var desciption = error.GetProperty("description").ToString();

        Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        Assert.Equal(UserError.IsNotAdm.Description, desciption);
    }
}
