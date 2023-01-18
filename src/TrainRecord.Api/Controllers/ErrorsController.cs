using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;

namespace TrainRecord.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ApiController
    {
        [Route("/error")]
        public IActionResult ErrorHandle()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            var errorUnexpected = Error.Unexpected("ErrorHandle.Unexpected", exception?.Message);
            var erros = new List<Error>() { errorUnexpected };

            return ProblemErrors(erros);
        }
    }
}
