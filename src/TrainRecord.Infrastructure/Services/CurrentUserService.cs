using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrainRecord.Infrastructure.Interfaces;

namespace TrainRecord.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly HttpContext _httpContext;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext;
    }

    public string? UserIdFromRoute =>
        _httpContext?.Request.RouteValues.GetValueOrDefault("userId").ToString();
    public string? UserId => _httpContext?.User?.FindFirstValue(ClaimTypes.Sid);
}
