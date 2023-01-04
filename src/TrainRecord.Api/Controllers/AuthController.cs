using ErrorOr;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using TrainRecord.Application.RegisterUser;

namespace TrainRecord.Controllers;

[ApiController]
public class AuthController : ApiControllerBase
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserCommand userRegisterCommand)
    {
        var registerResult = await Mediator.Send(userRegisterCommand);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => BadRequest(errors)
        );
    }
}
