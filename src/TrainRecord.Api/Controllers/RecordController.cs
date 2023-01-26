using System.Net;
using System.Security.Claims;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.CreateActivity;
using TrainRecord.Application.CreateUserActivity;
using TrainRecord.Application.DeleteRecord;
using TrainRecord.Application.Errors;
using TrainRecord.Application.GetActivityByUserQuery;
using TrainRecord.Application.GetAllUserQuery;
using TrainRecord.Application.GetRecordQuery;
using TrainRecord.Application.GetUserByIdQuery;
using TrainRecord.Core.Common;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Requests;

namespace TrainRecord.Controllers;

[ApiController]
public class RecordController : ApiController
{
    [HttpDelete("{recordId}")]
    [Authorize(Policy = "OwnerResource")]
    public async Task<IActionResult> DeleteRecord(Guid recordId)
    {
        var query = new DeleteRecordCommand() { RecordId = recordId, };

        var registerResult = await Mediator.Send(query);

        return registerResult.Match<IActionResult>(
            result => NoContent(),
            errors => ProblemErrors(errors)
        );
    }
}
