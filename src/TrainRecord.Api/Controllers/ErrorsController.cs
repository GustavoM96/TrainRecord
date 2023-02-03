using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Core.Commum;

namespace TrainRecord.Controllers
{
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

            var errorMessage = exception?.Message;
            var unexpectedError = errorMessage is null
                ? Error.Unexpected()
                : Error.Unexpected(description: errorMessage);

            return ProblemUniqueError(unexpectedError);
        }
    }
}
