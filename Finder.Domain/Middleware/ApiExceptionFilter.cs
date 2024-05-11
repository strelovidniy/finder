using Finder.Data.Enums;
using Finder.Data.Enums.RichEnums;
using Finder.Domain.Exceptions;
using Finder.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Finder.Domain.Middleware;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        StatusCode statusCode = default;

        switch (context.Exception)
        {
            case ApiException apiException:
                logger.LogError(apiException, ErrorMessage.ApiExceptionOccurred);

                statusCode = apiException.StatusCode;

                break;

            case ValidationException validationException:
                logger.LogWarning(validationException, ErrorMessage.ValidationExceptionOccurred);

                int.TryParse(validationException.Errors.FirstOrDefault()?.ErrorCode, out var errorCode);

                statusCode = (StatusCode) errorCode;

                break;

            default:
                logger.LogCritical(context.Exception, ErrorMessage.InternalServerErrorOccurred);

                break;
        }

        context.Result = new ObjectResult(new ApiErrorResult(statusCode));

        context.HttpContext.Response.StatusCode = statusCode switch
        {
            StatusCode.Unauthorized => 401,
            StatusCode.MethodNotAvailable => 500,
            _ => 400
        };

        context.HttpContext.Response.ContentType = ContentType.ApplicationProblem;
        context.ExceptionHandled = true;
    }
}