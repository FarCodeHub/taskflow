using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.SharedKernel.Domain;
using TaskFlow.SharedKernel.Validation;

namespace TaskFlow.Tasks.Api.Middleware;

/// <summary>
/// Handles unhandled exceptions globally and converts them to ProblemDetails responses.
/// This keeps controllers clean and provides consistent API error responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;





    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(
         exception,
         "Unhandled exception occurred while processing {Method} {Path}",
         httpContext.Request.Method,
         httpContext.Request.Path);

            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext httpContext,
        Exception exception)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException =>
                CreateValidationProblemDetails(validationException),

            DomainException domainException =>
                CreateDomainProblemDetails(domainException),

            _ => CreateServerErrorProblemDetails()
        };

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = problemDetails.Status ?? 500;

        var response = JsonSerializer.Serialize(problemDetails);

        await httpContext.Response.WriteAsync(response);
    }

    private static ProblemDetails CreateDomainProblemDetails(
        DomainException exception)
    {
        return new ProblemDetails
        {
            Type = "domain_error",
            Title = "Domain Error",
            Detail = exception.Message,
            Status = (int)HttpStatusCode.BadRequest
        };
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        ValidationException exception)
    {
        return new ValidationProblemDetails()
        {
            Type = "validation_error",
            Title = "Validation Error",
            Status = (int)HttpStatusCode.BadRequest
        };
    }

    private static ProblemDetails CreateServerErrorProblemDetails()
    {
        return new ProblemDetails
        {
            Type = "server_error",
            Title = "Server Error",
            Detail = "An unexpected error occurred.",
            Status = (int)HttpStatusCode.InternalServerError
        };
    }
}