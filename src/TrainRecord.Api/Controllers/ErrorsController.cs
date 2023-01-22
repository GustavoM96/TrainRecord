using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
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
            var erros = new List<Error>();

            if (exception is ValidationException validationException)
            {
                return ProblemErrors(validationException.Errors);
            }

            erros.Add(Error.Unexpected("ErrorHandle.Unexpected", exception?.Message));
            return ProblemErrors(erros);
        }
    }
}
