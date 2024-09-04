using System.Text.Json;
using TrainRecord.Core.Services.Auth;

namespace WebApp.Tests.Common;

public abstract class CustomClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory _webApplicationFactory;

    protected CustomClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _webApplicationFactory = webApplicationFactory;
    }

    protected ApiTokenResponse GetAdmToken() => _webApplicationFactory.TokenAdm;

    protected ApiTokenResponse GetUserToken() => _webApplicationFactory.TokenUser;

    protected void AddAuthentication(string key)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", key);
    }

    protected static async Task<JsonElement> GetDataPropertyAsync(
        HttpResponseMessage httpResponseMessage
    )
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var body = JsonDocument.Parse(content);
        return body.RootElement.GetProperty("data");
    }

    protected static async Task<JsonElement> GetSingleErrorPropertyAsync(
        HttpResponseMessage httpResponseMessage
    )
    {
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var body = JsonDocument.Parse(content);
        return body.RootElement.GetProperty("errors").EnumerateArray().Single();
    }
}
