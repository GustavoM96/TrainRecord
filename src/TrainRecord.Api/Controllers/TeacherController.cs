using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
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
    public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
    {
        var query = new GetAllUserQuery() { Pagination = pagination, Role = Role.Teacher };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet("{userId}/[action]")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> Student([FromQuery] Pagination pagination, Guid userId)
    {
        var query = new GetAllStudentByTeacherQuery()
        {
            Pagination = pagination,
            TeacherId = userId
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpDelete("{userId}/Student/{studentId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> RemoveStudentFromTeacher(Guid userId, Guid studentId)
    {
        var query = new DeleteTeacherStudentCommand()
        {
            StudentId = studentId,
            TeacherId = userId,
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => NoContent(),
            errors => ProblemErrors(errors)
        );
    }
}
