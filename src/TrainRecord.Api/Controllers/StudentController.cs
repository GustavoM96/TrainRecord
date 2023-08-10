using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.UserCommand;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;

namespace TrainRecord.Controllers;

[ApiController]
public class StudentController : ApiController
{
    [HttpGet("{userId}/Teacher")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllTeacher(
        [FromQuery] Pagination pagination,
        Guid userId,
        CancellationToken cs
    )
    {
        var query = new GetAllTeacherByStudentQuery(new(userId), pagination);
        return await SendOk(query, cs);
    }

    [HttpDelete("{userId}/Teacher/{teacherId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> RemoveTeacherFromStudent(
        Guid userId,
        Guid teacherId,
        CancellationToken cs
    )
    {
        var command = new DeleteTeacherStudentCommand(new(teacherId), new(userId));
        return await SendNoContent(command, cs);
    }

    [HttpPost("{userId}/Teacher/{teacherId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> AddTeacher(Guid userId, Guid teacherId, CancellationToken cs)
    {
        var command = new CreateTeacherStudentCommand(new(teacherId), new(userId));
        return await SendCreated(command, cs);
    }
}
