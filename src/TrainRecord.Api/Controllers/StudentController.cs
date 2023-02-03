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
    [HttpGet("{userId}/[action]")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> Teacher([FromQuery] Pagination pagination, Guid userId)
    {
        var query = new GetAllTeacherByStudentQuery()
        {
            Pagination = pagination,
            StudentId = userId
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpDelete("{userId}/Teacher/{teacherId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> RemoveTeacherFromStudent(Guid userId, Guid teacherId)
    {
        var query = new DeleteTeacherStudentCommand()
        {
            StudentId = userId,
            TeacherId = teacherId,
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => NoContent(),
            errors => ProblemErrors(errors)
        );
    }

    [HttpPost("{userId}/Teacher/{teacherId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> AddTeacher(Guid userId, Guid teacherId)
    {
        var query = new CreateTeacherStudentCommand()
        {
            StudentId = userId,
            TeacherId = teacherId,
        };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }
}
