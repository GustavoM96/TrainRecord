﻿using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TrainRecord.Infrastructure.Interfaces.Repositories;

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

    protected OkObjectResult OkResult(object? obj)
    {
        UnitOfWork.SaveChangesAsync().Wait();
        return base.Ok(obj);
    }

    protected CreatedAtActionResult CreatedResult(string? actionName, object? obj)
    {
        UnitOfWork.SaveChangesAsync().Wait();
        return base.CreatedAtAction(actionName, obj);
    }

    protected CreatedAtActionResult CreatedResult(
        string? actionName,
        object? routeValues,
        object? obj
    )
    {
        UnitOfWork.SaveChangesAsync().Wait();
        return base.CreatedAtAction(actionName, routeValues, obj);
    }

    protected NoContentResult NoContentResult()
    {
        UnitOfWork.SaveChangesAsync().Wait();
        return base.NoContent();
    }
}
