using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.UserCommand;
using TrainRecord.Application.UserQuery;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
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
        [FromQuery] TeacherQueryRequest teacherQueryRequest
    )
    {
        var userQueryRequest = teacherQueryRequest.Adapt<UserQueryRequest>();
        userQueryRequest.Role = Role.Teacher;

        var query = new GetAllUserQuery()
        {
            Pagination = pagination,
            UserQueryRequest = userQueryRequest
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
    }

    [HttpGet("{userId}/Student")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> GetAllStudent([FromQuery] Pagination pagination, Guid userId)
    {
        var query = new GetAllStudentByTeacherQuery()
        {
            Pagination = pagination,
            TeacherId = new(userId)
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
    }

    [HttpDelete("{userId}/Student/{studentId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> RemoveStudentFromTeacher(Guid userId, Guid studentId)
    {
        var query = new DeleteTeacherStudentCommand()
        {
            StudentId = new(studentId),
            TeacherId = new(userId)
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(result => NoContent(), errors => ProblemErrors(errors));
    }

    [HttpPost("{userId}/Student/{studentId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> AddStudent(Guid userId, Guid studentId)
    {
        var query = new CreateTeacherStudentCommand()
        {
            StudentId = new(studentId),
            TeacherId = new(userId)
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match(
            result => CreatedAtAction("GetAllStudent", new { userId }, result),
            errors => ProblemErrors(errors)
        );
    }
}
