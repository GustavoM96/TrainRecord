using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.CreateTeacherStudent;
using TrainRecord.Application.GetAllUserQuery;
using TrainRecord.Core.Common;
using TrainRecord.Core.Enum;

namespace TrainRecord.Controllers;

[ApiController]
public class TeacherController : ApiController
{
    [HttpPost("{teacherId}/[action]/{userId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> Student(Guid userId, Guid teacherId)
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
}
