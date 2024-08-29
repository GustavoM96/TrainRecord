using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Application.Requests;
using TrainRecord.Application.UserCommand;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;

namespace TrainRecord.Controllers;

[ApiController]
public class StudentController : ApiController
{
    [HttpGet("{userId}/Teacher")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> GetAllTeacher(
        [FromQuery] Pagination pagination,
        Guid userId,
        CancellationToken ct
    )
    {
        var query = new GetAllTeacherByStudentQuery(new(userId), pagination);
        return await SendOk(query, ct);
    }

    [HttpDelete("{userId}/Teacher/{teacherId}")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> RemoveTeacherFromStudent(
        Guid userId,
        Guid teacherId,
        CancellationToken ct
    )
    {
        var command = new DeleteTeacherStudentCommand(new(teacherId), new(userId));
        return await SendNoContent(command, ct, new() { UseSqlTransaction = true });
    }

    [HttpPost("{userId}/Teacher/{teacherId}")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> AddTeacher(Guid userId, Guid teacherId, CancellationToken ct)
    {
        var command = new CreateTeacherStudentCommand(new(teacherId), new(userId));
        return await SendCreated(command, ct);
    }

    [HttpGet("{userId}/Activity")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> GetAllActivity(
        Guid userId,
        [FromQuery] Pagination pagination,
        CancellationToken ct
    )
    {
        var query = new GetActivityByUserQuery(new(userId), pagination);
        return await SendOk(query, ct);
    }

    [HttpGet("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> GetRecordByActivityId(
        Guid userId,
        Guid activityId,
        [FromQuery] Pagination pagination,
        CancellationToken ct
    )
    {
        var query = new GetRecordByActivityIdQuery(new(userId), new(activityId), pagination);
        return await SendOk(query, ct);
    }

    [HttpDelete("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> DeleteAllRecord(
        Guid userId,
        Guid activityId,
        CancellationToken ct
    )
    {
        var command = new DeleteAllRecordByUserActivityCommand(new(userId), new(activityId));
        return await SendNoContent(command, ct, new() { UseSqlTransaction = true });
    }

    [HttpPost("{userId}/Activity/{activityId}/Record")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> CreateRecord(
        [FromRoute] Guid activityId,
        [FromRoute] Guid userId,
        [FromBody] CreateUserActivityRequest createUserActivityResquest,
        CancellationToken ct
    )
    {
        var command = new CreateUserActivityCommand(
            new(userId),
            createUserActivityResquest.TeacherId is null
                ? null
                : new(createUserActivityResquest.TeacherId.Value),
            new(activityId),
            createUserActivityResquest.Weight,
            createUserActivityResquest.Repetition,
            createUserActivityResquest.Serie,
            createUserActivityResquest.TrainGroup,
            createUserActivityResquest.TrainName,
            createUserActivityResquest.Time
        );

        return await SendCreated(command, ct);
    }

    [HttpGet("{userId}/Record")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> GetAllRecord(
        Guid userId,
        [FromQuery] Pagination pagination,
        CancellationToken ct
    )
    {
        var query = new GetAllRecordQuery(new(userId), null, pagination);
        return await SendOk(query, ct);
    }

    [HttpDelete("{userId}/Record/{recordId}")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> DeleteRecord(Guid recordId, Guid userId, CancellationToken ct)
    {
        var command = new DeleteRecordCommand(new(recordId), new(userId));
        return await SendNoContent(command, ct, new() { UseSqlTransaction = true });
    }
}
