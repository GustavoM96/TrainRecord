using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.AuthCommand;
using TrainRecord.Application.Requests;

namespace TrainRecord.Controllers;

[ApiController]
public class AuthController : ApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken cs)
    {
        return await SendCreated(command, cs);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cs)
    {
        return await SendOk(command, cs);
    }

    [HttpPatch("[action]")]
    public async Task<IActionResult> ChangePassword(
        UpdatePasswordRequest request,
        CancellationToken cs
    )
    {
        var loginUserCommand = new LoginUserCommand(request.Email, request.Password);
        var loginResult = await Mediator.Send(loginUserCommand, cs);

        var updatePasswordCommand = new UpdatePasswordCommand(request.Email, request.NewPassword);

        return loginResult.IsError
            ? ProblemErrors(loginResult.Errors)
            : await SendOk(updatePasswordCommand, cs);
    }
}
