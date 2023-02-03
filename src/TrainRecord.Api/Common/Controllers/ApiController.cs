using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainRecord.Core.Interfaces.Repositories;

namespace TrainRecord.Api.Common.Controller;

[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    private ISender _mediator = null!;
    private IRepositoryContext _context = null!;
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private IRepositoryContext _contextRepository =>
        _context ??= HttpContext.RequestServices.GetRequiredService<IRepositoryContext>();

    protected IActionResult ProblemErrors(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.Count == 1)
        {
            var firstError = errors[0];
            return ProblemUniqueError(firstError);
        }

        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in errors)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }

    protected IActionResult ProblemUniqueError(Error error)
    {
        if (error.Type == ErrorType.Validation)
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(error.Code, error.Description);

            return ValidationProblem(modelStateDictionary);
        }

        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }

    protected IActionResult Ok(object? obj)
    {
        _contextRepository.SaveChangesAsync();
        return base.Ok(obj);
    }

    protected IActionResult CreatedAtAction(string actionName, object? obj)
    {
        _contextRepository.SaveChangesAsync();
        return base.CreatedAtAction(actionName, obj);
    }

    protected IActionResult CreatedAtAction(string actionName, object? routeValues, object? obj)
    {
        _contextRepository.SaveChangesAsync();
        return base.CreatedAtAction(actionName, routeValues, obj);
    }

    protected IActionResult NoContent()
    {
        _contextRepository.SaveChangesAsync();
        return base.NoContent();
    }
}
