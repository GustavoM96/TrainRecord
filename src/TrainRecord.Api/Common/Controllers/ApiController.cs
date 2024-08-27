using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using TrainRecord.Api.Common.Builders;
using TrainRecord.Api.Common.Controllers;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Extensions;

namespace TrainRecord.Api.Common.Controller;

[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    private ISender _mediator = null!;
    private IUnitOfWork _unitOfWork = null!;
    private ILogger<ApiController> _logger = null!;
    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    private IUnitOfWork UnitOfWork =>
        _unitOfWork ??= HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

    private ILogger<ApiController> Logger =>
        _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<ApiController>>();

    protected async Task<IActionResult> SendOk<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        CancellationToken ct = default,
        ApiOptions? apiOptions = default
    )
    {
        var options = apiOptions ?? new();
        using IDbContextTransaction? transaction = options.UseSqlTransaction
            ? await UnitOfWork.BeginTransaction(ct)
            : null;

        var result = await GetResult(request, ct);
        if (result.IsError)
        {
            return ProblemErrors(result.Errors);
        }

        await UnitOfWork.SaveChangesAsync(ct);
        if (transaction is not null)
        {
            await UnitOfWork.CommitTransactionAsync(transaction, ct);
        }

        var response = new ApiOkResponse(result.Value, HttpContext.TraceIdentifier);
        return Ok(response);
    }

    protected async Task<IActionResult> SendNoContent<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        CancellationToken ct = default,
        ApiOptions? apiOptions = default
    )
    {
        var options = apiOptions ?? new();
        using IDbContextTransaction? transaction = options.UseSqlTransaction
            ? await UnitOfWork.BeginTransaction(ct)
            : null;

        var result = await GetResult(request, ct);
        if (result.IsError)
        {
            return ProblemErrors(result.Errors);
        }

        await UnitOfWork.SaveChangesAsync(ct);
        if (transaction is not null)
        {
            await UnitOfWork.CommitTransactionAsync(transaction, ct);
        }

        return NoContent();
    }

    protected async Task<IActionResult> SendCreated<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        CancellationToken ct = default,
        ApiOptions? apiOptions = null
    )
    {
        var options = apiOptions ?? new();
        using IDbContextTransaction? transaction = options.UseSqlTransaction
            ? await UnitOfWork.BeginTransaction(ct)
            : null;

        var result = await GetResult(request, ct);
        if (result.IsError)
        {
            return ProblemErrors(result.Errors);
        }

        await UnitOfWork.SaveChangesAsync(ct);
        if (transaction is not null)
        {
            await UnitOfWork.CommitTransactionAsync(transaction, ct);
        }

        var response = new ApiCreatedResponse(result.Value, HttpContext.TraceIdentifier);
        return CreatedAtAction(null, response);
    }

    private async Task<ErrorOr<TResponse>> GetResult<TResponse>(
        IRequest<ErrorOr<TResponse>> request,
        CancellationToken ct
    )
    {
        var timer = new Stopwatch();
        var result = await timer.GetTimeAsync(() => Mediator.Send(request, ct));
        var response = result.Value;

        Logger.LogInformation(
            "{Name} TraceId: {TraceId} TimeSpan: {Elapsed} UserID: {UserId} Request: {Request}, Response: {Response}",
            request.GetType().ToString(),
            HttpContext.TraceIdentifier,
            result.Elapsed,
            HttpContext?.User?.FindFirstValue(ClaimTypes.Sid) ?? "'no userId inserted'",
            JsonSerializer.Serialize((object)request),
            response.IsError
                ? JsonSerializer.Serialize(response.Errors)
                : JsonSerializer.Serialize(response.Value)
        );

        return response;
    }

    protected IActionResult ProblemErrors(List<Error> errors)
    {
        Logger.LogError(
            ProblemDetailsBuilder.UnhandledExceptionMsg + "TraceId: {traceId}",
            HttpContext.TraceIdentifier
        );

        if (errors.Count == 0)
        {
            throw new ArgumentException("A list of error cannot be empty");
        }

        var problemDetails = ProblemDetailsBuilder.Build(HttpContext.TraceIdentifier, errors);
        return new ObjectResult(problemDetails);
    }
}
