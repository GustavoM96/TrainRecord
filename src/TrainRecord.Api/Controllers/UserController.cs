using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;
using TrainRecord.Application.Requests;
using TrainRecord.Application.UserCommand;

namespace TrainRecord.Controllers;

[ApiController]
public class UserController : ApiController
{
    [HttpPost("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> CreateRecord(
        [FromRoute] Guid activityId,
        [FromRoute] Guid userId,
        [FromBody] CreateUserActivityRequest createUserActivityResquest,
        CancellationToken cs
    )
    {
        var command = new CreateUserActivityCommand(
            new(userId),
            new(activityId),
            createUserActivityResquest.Weight,
            createUserActivityResquest.Repetition,
            createUserActivityResquest.Serie
        );

        return await SendCreated(command, cs);
    }

    [HttpGet("{userId}/Activity")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllActivity(
        Guid userId,
        [FromQuery] Pagination pagination,
        CancellationToken cs
    )
    {
        var query = new GetActivityByUserQuery(new(userId), pagination);
        return await SendOk(query, cs);
    }

    [HttpGet("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllRecord(
        Guid userId,
        Guid activityId,
        [FromQuery] Pagination pagination,
        CancellationToken cs
    )
    {
        var query = new GetRecordQuery(new(userId), new(activityId), pagination);
        return await SendOk(query, cs);
    }

    [HttpGet]
    [Authorize(Policy = "IsAdm")]
    public async Task<IActionResult> GetAll(
        [FromQuery] Pagination pagination,
        [FromQuery] UserQueryRequest userQueryRequest,
        CancellationToken cs
    )
    {
        var query = new GetAllUserQuery(userQueryRequest, pagination);
        return await SendOk(query, cs);
    }

    [HttpGet("{userId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetById(Guid userId, CancellationToken cs)
    {
        var query = new GetUserByIdQuery(new(userId));
        return await SendOk(query, cs);
    }

    [HttpDelete("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> DeleteAllRecord(
        Guid userId,
        Guid activityId,
        CancellationToken cs
    )
    {
        var command = new DeleteAllRecordByUserActivityCommand(new(userId), new(activityId));
        return await SendNoContent(command, cs);
    }

    [HttpPatch("{userId}")]
    [Authorize(Policy = "OwnerResource")]
    [Consumes("application/merge-patch+json")]
    public async Task<IActionResult> Update(
        Guid userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken cs
    )
    {
        var command = new UpdateUserCommand(new(userId), request.FirstName, request.LastName);
        return await SendOk(command, cs);
    }

    [HttpDelete("{userId}/Record/{recordId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> DeleteRecord(Guid recordId, CancellationToken cs)
    {
        var command = new DeleteRecordCommand(new(recordId));
        return await SendNoContent(command, cs);
    }
}
