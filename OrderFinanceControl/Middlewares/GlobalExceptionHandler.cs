using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderFinanceControl.Exceptions;

namespace OrderFinanceControl.Middlewares;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {

        var problem = exception switch
        {
            NotFoundException ex => CreateProblem(
                StatusCodes.Status404NotFound,
                "Resource not found",
                ex.Message,
                context),

            AlreadyExistsException ex => CreateProblem(
                StatusCodes.Status409Conflict,
                "Resource already exists",
                ex.Message,
                context),

            ConflictException ex => CreateProblem(
                StatusCodes.Status409Conflict,
                "Conflict",
                ex.Message,
                context),

            _ => CreateProblem(
                StatusCodes.Status500InternalServerError,
                "Internal server error",
                "An unexpected error occurred.",
                context)
        };

        context.Response.StatusCode = problem.Status!.Value;
        await context.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblem(
        int status,
        string title,
        string detail,
        HttpContext context)
    {
        return new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = status,
            Instance = context.Request.Path
        };
    }
}
