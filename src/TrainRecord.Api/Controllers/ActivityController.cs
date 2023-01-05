using ErrorOr;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using TrainRecord.Application.CreateActivity;
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
}
