using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrainRecord.Infrastructure.Interfaces;

namespace TrainRecord.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserIdFromRoute =>
        _httpContextAccessor.HttpContext?.Request.RouteValues
            .GetValueOrDefault("userId")
            .ToString();
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Sid);
    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

    public bool IsAdmin => Role == "Adm";
    public bool IsOwnerResource => UserId == UserIdFromRoute && UserId is not null;
}
