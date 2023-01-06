using ErrorOr;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Base;
using TrainRecord.Application.LoginUser;
using TrainRecord.Application.RegisterUser;

namespace TrainRecord.Controllers;

[ApiController]
public class AuthController : ApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserCommand userRegisterCommand)
    {
        var registerResult = await Mediator.Send(userRegisterCommand);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
    {
        var registerResult = await Mediator.Send(loginUserCommand);

        return registerResult.Match<IActionResult>(
            result => Ok(result),
            errors => ProblemErrors(errors)
        );
    }
}
