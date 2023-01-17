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
using TrainRecord.Application.GetAllUserQuery;
using TrainRecord.Application.GetUserActivity;
using TrainRecord.Application.LoginUser;
using TrainRecord.Application.RegisterUser;
using TrainRecord.Core.Common;
using TrainRecord.Core.Enum;

namespace TrainRecord.Controllers;

[ApiController]
public class UserController : ApiController
{
    [HttpPost("{userId}/Activity/{id}/[action]")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> Record(
        [FromRoute] Guid id,
        [FromRoute] Guid userId,
        [FromBody] CreateUserActivityRequest createUserActivityResquest
    )
    {
        var command = new CreateUserActivityCommand()
        {
            Weight = createUserActivityResquest.Weight,
            Repetition = createUserActivityResquest.Repetition,
            ActivityId = id,
            UserId = userId
        };

        var registerResult = await Mediator.Send(command);

        return registerResult.Match<IActionResult>(
            result => CreatedAtAction($"Activity", new { userId }, result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet("{userId}/[action]")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> Activity(Guid userId, [FromQuery] Pagination pagination)
    {
        var query = new GetUserActivityQuery() { UserId = userId, Pagination = pagination };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet]
    [Authorize(Roles = "Adm")]
    public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
    {
        var query = new GetAllUserQuery() { Pagination = pagination };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }
}
