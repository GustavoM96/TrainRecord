using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.Requests;
using TrainRecord.Application.UserCommand;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;

namespace TrainRecord.Controllers;

[ApiController]
public class UserController : ApiController
{
    [HttpGet]
    [Authorize(Policy = "IsAdm")]
    public async Task<IActionResult> GetAll(
        [FromQuery] Pagination pagination,
        [FromQuery] UserQueryRequest userQueryRequest,
        CancellationToken ct
    )
    {
        var query = new GetAllUserQuery(userQueryRequest, pagination);
        return await SendOk(query, ct);
    }

    [HttpGet("{userId}")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken ct)
    {
        var query = new GetUserByIdQuery(new(userId));
        return await SendOk(query, ct);
    }

    [HttpPatch("{userId}")]
    [Authorize(Policy = "ResourceOwner")]
    [Consumes("application/merge-patch+json")]
    public async Task<IActionResult> Update(
        Guid userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken ct
    )
    {
        var command = new UpdateUserCommand(new(userId), request.FirstName, request.LastName);
        return await SendOk(command, ct, new() { UseSqlTransaction = false });
    }
}
