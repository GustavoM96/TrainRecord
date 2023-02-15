using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.ActivityCommand;
using TrainRecord.Application.ActivityQuery;
using TrainRecord.Core.Common;

namespace TrainRecord.Controllers;

[ApiController]
public class ActivityController : ApiController
{
    [HttpPost]
    [Authorize(Policy = "IsAdm")]
    public async Task<IActionResult> Create(CreateActivityCommand createActivityCommand)
    {
        var registerResult = await Mediator.Send(createActivityCommand);

        return registerResult.Match(
            result => CreatedAtAction("GetAll", result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
    {
        var getAllActivityQuery = new GetAllActivityQuery() { Pagination = pagination };
        var registerResult = await Mediator.Send(getAllActivityQuery);

        return registerResult.Match(result => Ok(result), errors => ProblemErrors(errors));
    }
}
