using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrainRecord.Common.Errors;
using TrainRecord.Core.Exceptions;
using TrainRecord.Core.Extensions;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Infrastructure.Services.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private HttpContext? Context => _httpContextAccessor.HttpContext;

    public string? UserId => Context?.User?.FindFirstValue(ClaimTypes.Sid);
    public string? UserEmail => Context?.User?.FindFirstValue(ClaimTypes.Email);
    public string? Role => Context?.User?.FindFirstValue(ClaimTypes.Role);
    public bool IsAdmin => Role == "Adm";
    public bool IsResourceOwner => GetUserIdByRoute().EqualsIgnoreCase(UserId);

    public string GetUserIdByRoute()
    {
        var userId = Context?.Request.RouteValues.GetValueOrDefault("userId");

        if (userId is null)
        {
            throw new RequestException(RequestError.UserIdNotFound);
        }
        return userId.ToString()!;
    }
}
