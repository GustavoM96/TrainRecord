using System.Net;
using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Base;
using TrainRecord.Application.CreateActivity;
using TrainRecord.Application.CreateUserActivity;
using TrainRecord.Application.Errors;
using TrainRecord.Application.GetUserActivity;
using TrainRecord.Application.LoginUser;
using TrainRecord.Application.RegisterUser;
using TrainRecord.Core.Enum;

namespace TrainRecord.Controllers;

[ApiController]
public class ActivityController : ApiController
{
    [HttpPost]
    [Authorize(Roles = "Adm")]
    public async Task<IActionResult> Create(CreateActivityCommand createActivityCommand)
    {
        var registerResult = await Mediator.Send(createActivityCommand);

        return registerResult.Match<IActionResult>(
            result => CreatedAtAction("GetActivitiesByUser", result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpPost("{id}/[action]")]
    [Authorize]
    public async Task<IActionResult> Record(
        [FromRoute] Guid id,
        [FromBody] CreateUserActivityRequest createUserActivityResquest
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);

        var command = new CreateUserActivityCommand()
        {
            Weight = createUserActivityResquest.Weight,
            Repetition = createUserActivityResquest.Repetition,
            ActivityId = id,
            UserId = new Guid(userId)
        };

        var registerResult = await Mediator.Send(command);

        return registerResult.Match<IActionResult>(
            result => CreatedAtAction("GetActivitiesByUser", result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> GetActivitiesByUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.Sid);
        var query = new GetUserActivityQuery() { UserId = new Guid(userId) };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }
}
