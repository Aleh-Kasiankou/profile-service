using System.Text;
using System.Text.Json;
using FluentValidation;
using Idt.Profiles.Shared.Exceptions.BaseExceptions;
using Serilog;

namespace Idt.Profiles.Api.Middleware.ExceptionHandling;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private const string UnhandledExceptionMessage =
        "The server failed to handle this request. Please try again in a few minutes. If this error persists, please contact our support team.";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException e)
        {
            Log.Debug(e, "Validation exception occured");
            var errorMessage = string.Join(". ", e.Errors);
            await WriteExceptionMessageToResponse(errorMessage, StatusCodes.Status400BadRequest, context);
        }
        catch (ClientRelatedException e)
        {
            Log.Information(e, "Client-related exception occured");
            await WriteExceptionMessageToResponse(e.Message, StatusCodes.Status400BadRequest, context);
        }
        catch (ConcurrencyException e)
        {
            Log.Warning(e, "Concurrency exception occured while working with database");
            await WriteExceptionMessageToResponse(e.Message, StatusCodes.Status400BadRequest, context);
        }
        catch (RollbackSuccessException e)
        {
            Log.Warning(e, "Rollback occured while working with database");
            await WriteExceptionMessageToResponse(e.Message, StatusCodes.Status500InternalServerError, context);
        }
        catch (ServerRelatedException e)
        {
            Log.Error(e, "Server exception occured");
            await WriteExceptionMessageToResponse(UnhandledExceptionMessage, StatusCodes.Status500InternalServerError,
                context);
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Unhandled server exception occured");
            await WriteExceptionMessageToResponse(UnhandledExceptionMessage, StatusCodes.Status500InternalServerError,
                context);
        }
    }

    private async Task WriteExceptionMessageToResponse(string message, int code, HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = code;
        var jsonMessage = JsonSerializer.Serialize(message);
        await context.Response.WriteAsync(jsonMessage, Encoding.UTF8);
    }
}