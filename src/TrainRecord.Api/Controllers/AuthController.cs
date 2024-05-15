using Microsoft.AspNetCore.Mvc;
using TrainRecord.Api.Common.Controller;
using TrainRecord.Application.AuthCommand;
using TrainRecord.Application.Requests;

namespace TrainRecord.Controllers;

[ApiController]
public class AuthController : ApiController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken ct)
    {
        return await SendCreated(command, ct);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken ct)
    {
        return await SendOk(command, ct);
    }

    [HttpPatch("[action]")]
    public async Task<IActionResult> ChangePassword(
        UpdatePasswordRequest request,
        CancellationToken ct
    )
    {
        var loginUserCommand = new LoginUserCommand(request.Email, request.Password);
        var loginResult = await Mediator.Send(loginUserCommand, ct);

        if (loginResult.IsError)
        {
            ProblemErrors(loginResult.Errors);
        }

        var updatePasswordCommand = new UpdatePasswordCommand(
            request.Email,
            request.Password,
            request.NewPassword
        );
        return await SendOk(updatePasswordCommand, ct);
    }
}
