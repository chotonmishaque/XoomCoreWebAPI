using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;
using System.Text.Json;
using XoomCore.Application.Common.Exceptions;

namespace XoomCore.Services.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            context.Response.Headers.Remove("Server");
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            string exceptionMessage = "Something went wrong! Please contact with administrator!";
            switch (error)
            {
                case KeyNotFoundException ex:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    exceptionMessage = ex.Message;
                    break;
                case UnauthorizedException ex:
                    response.StatusCode = (int)ex.StatusCode;
                    exceptionMessage = ex.Message;
                    break;
                case ConflictException ex:
                    response.StatusCode = (int)ex.StatusCode;
                    exceptionMessage = ex.Message;
                    break;
                case ForbiddenException ex:
                    response.StatusCode = (int)ex.StatusCode;
                    exceptionMessage = ex.Message;
                    break;
                case InternalServerException ex:
                    response.StatusCode = (int)ex.StatusCode;
                    exceptionMessage = ex.Message;
                    break;
                case NotFoundException ex:
                    response.StatusCode = (int)ex.StatusCode;
                    exceptionMessage = ex.Message;
                    break;
                case CustomException ex:
                    // custom application error
                    response.StatusCode = (int)ex.StatusCode;
                    exceptionMessage = ex.ErrorMessages.ToString();
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    exceptionMessage = error.Message.ToString();
                    break;
            }

            Log.Error("ExceptionMiddleware: " + exceptionMessage);
            Log.Error("ExceptionMiddleware: " + error.StackTrace);

            var responseModel = CommonResponse<string>.CreateError(response.StatusCode, exceptionMessage);
            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}

