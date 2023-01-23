using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Exceptions;

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

            return ProblemUniqueError(Error.Unexpected(exception?.Message));
        }
    }
}
