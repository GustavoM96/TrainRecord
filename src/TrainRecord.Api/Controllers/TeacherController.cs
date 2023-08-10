using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.UserCommand;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;
using TrainRecord.Core.Enum;
using TrainRecord.Application.Requests;

namespace TrainRecord.Controllers;

[ApiController]
public class TeacherController : ApiController
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] Pagination pagination,
        [FromQuery] TeacherQueryRequest request,
        CancellationToken cs
    )
    {
        var userQueryRequest = request.Adapt<UserQueryRequest>();
        userQueryRequest.Role = Role.Teacher;

        var query = new GetAllUserQuery(userQueryRequest, pagination);
        return await SendOk(query, cs);
    }

    [HttpGet("{userId}/Student")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllStudent(
        [FromQuery] Pagination pagination,
        Guid userId,
        CancellationToken cs
    )
    {
        var query = new GetAllStudentByTeacherQuery(new(userId), pagination);
        return await SendOk(query, cs);
    }

    [HttpDelete("{userId}/Student/{studentId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> RemoveStudentFromTeacher(
        Guid userId,
        Guid studentId,
        CancellationToken cs
    )
    {
        var command = new DeleteTeacherStudentCommand(new(userId), new(studentId));
        return await SendNoContent(command, cs);
    }

    [HttpPost("{userId}/Student/{studentId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> AddStudent(Guid userId, Guid studentId, CancellationToken cs)
    {
        var command = new CreateTeacherStudentCommand(new(userId), new(studentId));
        return await SendCreated(command, cs);
    }
}
