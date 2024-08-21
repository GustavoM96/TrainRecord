using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainRecord.Application.Interfaces.Repositories;

namespace TrainRecord.Api.Common.Controller;

[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    private ISender _mediator = null!;
    private IUnitOfWork _unitOfWork = null!;
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private IUnitOfWork UnitOfWork =>
        _unitOfWork ??= HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

    protected async Task<IActionResult> SendOk<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        CancellationToken ct = default
    )
    {
        var result = await Mediator.Send(request, ct);

        if (result.IsError)
        {
            return ProblemErrors(result.Errors);
        }

        await UnitOfWork.SaveChangesAsync(ct);
        return Ok(result.Value);
    }

    protected async Task<IActionResult> SendNoContent<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        CancellationToken ct = default
    )
    {
        var result = await Mediator.Send(request, ct);

        if (result.IsError)
        {
            return ProblemErrors(result.Errors);
        }

        await UnitOfWork.SaveChangesAsync(ct);
        return NoContent();
    }

    protected async Task<IActionResult> SendCreatedBase<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        string? actionName = null,
        object? routeValues = null,
        CancellationToken ct = default
    )
    {
        var result = await Mediator.Send(request, ct);

        if (result.IsError)
        {
            return ProblemErrors(result.Errors);
        }

        await UnitOfWork.SaveChangesAsync(ct);
        return CreatedAtAction(actionName, routeValues, result.Value);
    }

    protected async Task<IActionResult> SendCreated<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        CancellationToken ct = default
    )
    {
        return await SendCreatedBase(request, ct: ct);
    }

    protected IActionResult ProblemErrors(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return UnexpectedProblem();
        }

        var statusCode = StatusCodes.Status400BadRequest;
        if (errors.Count == 1)
        {
            statusCode = errors[0].Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest,
            };
        }

        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(
            statusCode: statusCode,
            modelStateDictionary: modelStateDictionary
        );
    }

    protected IActionResult UnexpectedProblem(string? title = null)
    {
        return Problem(
            title: title ?? "Internal Error Unexpected",
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
}
