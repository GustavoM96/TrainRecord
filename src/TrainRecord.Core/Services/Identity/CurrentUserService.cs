using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Throw;
using TrainRecord.Common.Errors;
using TrainRecord.Core.Exceptions;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Infrastructure.Services.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpContext? _httpContext => _httpContextAccessor.HttpContext;

    public string? UserId => _httpContext?.User?.FindFirstValue(ClaimTypes.Sid);
    public string? UserEmail => _httpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public string? Role => _httpContext?.User?.FindFirstValue(ClaimTypes.Role);
    public bool IsAdmin => Role == "Adm";
    public bool IsOwnerResource => GetUserIdByRoute().EqualsIgnoreCase(UserId);

    public string GetUserIdByRoute()
    {
        var userId = _httpContext?.Request.RouteValues.GetValueOrDefault("userId");

        userId.ThrowIfNull(() => new RequestException(RequestError.UserIdNotFound));
        return userId.ToString()!;
    }
}
