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
        [FromBody] CreateUserActivityRequest createUserActivityResquest
    )
    {
        var command = new CreateUserActivityCommand()
        {
            Weight = createUserActivityResquest.Weight,
            Repetition = createUserActivityResquest.Repetition,
            Serie = createUserActivityResquest.Serie,
            ActivityId = new(activityId),
            UserId = new(userId)
        };

        var result = await Mediator.Send(command);

        return result.Match(
            result => CreatedResult($"GetAllActivity", new { userId }, result),
            ProblemErrors
        );
    }

    [HttpGet("{userId}/Activity")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllActivity(Guid userId, [FromQuery] Pagination pagination)
    {
        var query = new GetActivityByUserQuery() { UserId = new(userId), Pagination = pagination };

        var result = await Mediator.Send(query);

        return result.Match(OkResult, ProblemErrors);
    }

    [HttpGet("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllRecord(
        Guid userId,
        Guid activityId,
        [FromQuery] Pagination pagination
    )
    {
        var query = new GetRecordQuery()
        {
            UserId = new(userId),
            ActivityId = new(activityId),
            Pagination = pagination
        };

        var result = await Mediator.Send(query);

        return result.Match(OkResult, ProblemErrors);
    }

    [HttpGet]
    [Authorize(Policy = "IsAdm")]
    public async Task<IActionResult> GetAll(
        [FromQuery] Pagination pagination,
        [FromQuery] UserQueryRequest userQueryRequest
    )
    {
        var query = new GetAllUserQuery()
        {
            Pagination = pagination,
            UserQueryRequest = userQueryRequest
        };

        var result = await Mediator.Send(query);

        return result.Match(OkResult, ProblemErrors);
    }

    [HttpGet("{userId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var query = new GetUserByIdQuery() { UserId = new(userId) };

        var result = await Mediator.Send(query);

        return result.Match(OkResult, ProblemErrors);
    }

    [HttpDelete("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> DeleteAllRecord(Guid userId, Guid activityId)
    {
        var query = new DeleteAllRecordByUserActivityCommand()
        {
            UserId = new(userId),
            ActivityId = new(activityId)
        };

        var result = await Mediator.Send(query);

        return result.Match(result => NoContentResult(), ProblemErrors);
    }

    [HttpPatch("{userId}")]
    [Authorize(Policy = "OwnerResource")]
    [Consumes("application/merge-patch+json")]
    public async Task<IActionResult> Update(Guid userId, [FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserId = new(userId)
        };

        var result = await Mediator.Send(command);
        return result.Match(OkResult, ProblemErrors);
    }

    [HttpDelete("{userId}/Record/{recordId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> DeleteRecord(Guid recordId)
    {
        var query = new DeleteRecordCommand() { RecordId = new(recordId), };

        var result = await Mediator.Send(query);

        return result.Match(result => NoContentResult(), ProblemErrors);
    }
}
