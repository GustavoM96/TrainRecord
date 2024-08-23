using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.Requests;
using TrainRecord.Application.UserCommand;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;
using TrainRecord.Core.Enum;

namespace TrainRecord.Controllers;

[ApiController]
public class TeacherController : ApiController
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
        [FromQuery] Pagination pagination,
        [FromQuery] TeacherQueryRequest request,
        CancellationToken ct
    )
    {
        var userQueryRequest = request.Adapt<UserQueryRequest>();
        userQueryRequest.Role = Role.Teacher;

        var query = new GetAllUserQuery(userQueryRequest, pagination);
        return await SendOk(query, ct);
    }

    [HttpGet("{userId}/Student")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> GetAllStudent(
        [FromQuery] Pagination pagination,
        Guid userId,
        CancellationToken ct
    )
    {
        var query = new GetAllStudentByTeacherQuery(new(userId), pagination);
        return await SendOk(query, ct);
    }

    [HttpDelete("{userId}/Student/{studentId}")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> RemoveStudentFromTeacher(
        Guid userId,
        Guid studentId,
        CancellationToken ct
    )
    {
        var command = new DeleteTeacherStudentCommand(new(userId), new(studentId));
        return await SendNoContent(command, ct);
    }

    [HttpPost("{userId}/Student/{studentId}")]
    [Authorize(Policy = "ResourceOwner")]
    public async Task<IActionResult> AddStudent(Guid userId, Guid studentId, CancellationToken ct)
    {
        var command = new CreateTeacherStudentCommand(new(userId), new(studentId));
        return await SendCreated(command, ct);
    }
}
