using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.AuthCommand;
using TrainRecord.Application.Requests;

namespace TrainRecord.Controllers;

[ApiController]
public class AuthController : ApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserCommand userRegisterCommand)
    {
        var result = await Mediator.Send(userRegisterCommand);

        return result.Match(
            result => CreatedAtAction("Login", result),
            errors => ProblemErrors(errors)
        );
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserCommand loginUserCommand)
    {
        var result = await Mediator.Send(loginUserCommand);

        return result.Match(result => Ok(result), errors => ProblemErrors(errors));
    }

    [HttpPatch("[action]")]
    public async Task<IActionResult> ChangePassword(UpdatePasswordRequest request)
    {
        var loginUserCommand = new LoginUserCommand()
        {
            Email = request.Email,
            Password = request.Password
        };
        var loginResult = await Mediator.Send(loginUserCommand);

        if (loginResult.IsError)
        {
            return ProblemErrors(loginResult.Errors);
        }

        var updateCommand = new UpdatePasswordCommand()
        {
            Email = request.Email,
            NewPassword = request.NewPassword
        };
        var updateResult = await Mediator.Send(updateCommand);

        return updateResult.Match(result => NoContent(), errors => ProblemErrors(errors));
    }
}
