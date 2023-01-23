using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Infrastructure.Services.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserIdFromRoute =>
        _httpContext?.Request.RouteValues.GetValueOrDefault("userId")?.ToString();
    public string? UserId => _httpContext?.User?.FindFirstValue(ClaimTypes.Sid);
    public string? Role => _httpContext?.User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAdmin => Role == "Adm";
    public bool IsOwnerResource => UserId == UserIdFromRoute && UserId is not null;
    private HttpContext? _httpContext => _httpContextAccessor.HttpContext;
}
