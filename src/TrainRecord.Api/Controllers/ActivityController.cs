using ErrorOr;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using TrainRecord.Application.CreateActivity;
using TrainRecord.Application.CreateUserActivity;
using TrainRecord.Application.LoginUser;
using TrainRecord.Application.RegisterUser;

namespace TrainRecord.Controllers;

[ApiController]
public class ActivityController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateActivityCommand createActivityCommand)
    {
        var registerResult = await Mediator.Send(createActivityCommand);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => BadRequest(errors)
        );
    }

    [HttpPost("{id}/[action]")]
    public async Task<IActionResult> Record(
        [FromRoute] Guid id,
        [FromBody] CreateUserActivityRequest createUserActivityResquest
    )
    {
        var command = new CreateUserActivityCommand()
        {
            Weight = createUserActivityResquest.Weight,
            Repetition = createUserActivityResquest.Repetition
        };

        var registerResult = await Mediator.Send(command);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => BadRequest(errors)
        );
    }
}
