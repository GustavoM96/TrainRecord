using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace TrainRecord.Api.Common.Builders;

public static class ProblemDetailsBuilder
{
    public const string UnhandledExceptionMsg =
        "An unhandled exception has occurred while executing the request.";
    public const string OneOrMoreErrosMsg =
        "One or more errors has occurred while executing the request.";

    public static ProblemDetails Build(string traceId, Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = UnhandledExceptionMsg,
            Detail = exception.Message,
        };

        problemDetails.Extensions["dateTime"] = DateTime.Now;
        problemDetails.Extensions["traceId"] = traceId;

        problemDetails.Extensions["errors"] = new List<Error>()
        {
            Error.Unexpected(exception.GetType().ToString(), exception.Message),
        };

        problemDetails.Extensions["exceptionInfo"] = exception.ToString();
        problemDetails.Extensions["exceptionData"] = exception.Data;

        return problemDetails;
    }

    public static ProblemDetails Build(string traceId, List<Error> errors)
    {
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

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = OneOrMoreErrosMsg,
            Detail = string.Join(" | ", errors.Select(e => e.Description)),
        };

        problemDetails.Extensions["dateTime"] = DateTime.Now;
        problemDetails.Extensions["traceId"] = traceId;
        problemDetails.Extensions["errors"] = errors;

        return problemDetails;
    }
}
