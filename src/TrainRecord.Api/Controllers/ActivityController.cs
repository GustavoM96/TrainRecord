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
    public async Task<IActionResult> Create(CreateActivityCommand command, CancellationToken ct)
    {
        return await SendCreated(command, ct);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] Pagination pagination, CancellationToken ct)
    {
        var query = new GetAllActivityQuery() { Pagination = pagination };
        return await SendOk(query, ct);
    }
}
