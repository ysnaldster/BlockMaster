using System.Net;
using System.Text.Json;
using BlockMaster.Domain.Exceptions.BadRequestException;
using BlockMaster.Domain.Exceptions.ConflictException;
using BlockMaster.Domain.Exceptions.NotFoundException;
using Serilog;

namespace BlockMaster.Api.Middleware;

public class ExceptionMiddleware
{
    #region private attributes

    private readonly RequestDelegate _requestDelegate;
    private const string ContentType = "application/json";

    #endregion

    #region public methods

    public ExceptionMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _requestDelegate(httpContext);
        }
        catch (NotFoundGeneralException e)
        {
            await CustomHelperException(httpContext, HttpStatusCode.NotFound, e);
        }
        catch (BadRequestGeneralException e)
        {
            await CustomHelperException(httpContext, HttpStatusCode.BadRequest, e);
        }
        catch (UnauthorizedAccessException e)
        {
            await CustomHelperException(httpContext, HttpStatusCode.Unauthorized, e);
        }
        catch (ConflictGeneralException e)
        {
            await CustomHelperException(httpContext, HttpStatusCode.Conflict, e);
        }
        catch (Exception e)
        {
            await CustomHelperException(httpContext, HttpStatusCode.InternalServerError, e);
        }
    }

    #endregion

    #region private methods

    private static async Task CustomHelperException(HttpContext context, HttpStatusCode statusCode, Exception e)
    {
        Log.Error($"\nSomething went wrong, Message: {e.Message} \n Exception: {e.StackTrace} \n");
        var exceptionMessage = e.Message.Replace("{", "").Replace("}", "");
        var responseMessage = $"{statusCode}: {exceptionMessage}";

        await BuildMessageExceptionAsync(context, statusCode, responseMessage);
    }

    private static Task BuildMessageExceptionAsync(HttpContext context, HttpStatusCode statusCode, string? message)
    {
        context.Response.ContentType = ContentType;
        context.Response.StatusCode = (int)statusCode;
        var exception = new
        {
            HttpStatusCode = context.Response.StatusCode,
            Message = message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(exception));
    }

    #endregion
}