using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ApiController
{
    [Route("/error")]
    public IActionResult ErrorHandle()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is HandlerException handlerException)
        {
            return ProblemErrors(handlerException.Errors);
        }

        return UnexpectedProblem(exception?.Message);
    }
}
