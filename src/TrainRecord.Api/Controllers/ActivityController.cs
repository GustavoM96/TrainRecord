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
        var result = await Mediator.Send(createActivityCommand);

        return result.Match(
            result => CreatedResult("GetAll", result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
    {
        var getAllActivityQuery = new GetAllActivityQuery() { Pagination = pagination };
        var result = await Mediator.Send(getAllActivityQuery);

        return result.Match(result => OkResult(result), errors => ProblemErrors(errors));
    }
}
