using System.Net.Mime;
using LaDeak.JsonMergePatch.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.UserCommand;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Requests;

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
            ActivityId = activityId,
            UserId = userId
        };

        var registerResult = await Mediator.Send(command);

        return registerResult.Match(
            result => CreatedAtAction($"GetAllActivity", new { userId }, result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet("{userId}/Activity")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllActivity(Guid userId, [FromQuery] Pagination pagination)
    {
        var query = new GetActivityByUserQuery() { UserId = userId, Pagination = pagination };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
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
            UserId = userId,
            ActivityId = activityId,
            Pagination = pagination
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
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

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
    }

    [HttpGet("{userId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetById(Guid userId)
    {
        var query = new GetUserByIdQuery() { UserId = userId };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
    }

    [HttpDelete("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> DeleteAllRecord(Guid userId, Guid activityId)
    {
        var query = new DeleteAllRecordByUserActivityCommand()
        {
            UserId = userId,
            ActivityId = activityId,
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => NoContent(), errors => ProblemErrors(errors));
    }

    [HttpPatch("{userId}")]
    [Authorize(Policy = "OwnerResource")]
    [Consumes("application/merge-patch+json")]
    public async Task<IActionResult> Update(Guid userId, [FromBody] Patch<User> patch)
    {
        var command = new UpdateUserCommand() { Patch = patch, UserId = userId };

        var registerResult = await Mediator.Send(command);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
    }

    [HttpDelete("{userId}/Record/{recordId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> DeleteRecord(Guid recordId)
    {
        var query = new DeleteRecordCommand() { RecordId = recordId, };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => NoContent(), errors => ProblemErrors(errors));
    }
}
