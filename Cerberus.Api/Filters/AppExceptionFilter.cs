using System;
using System.Net;
using Cerberus.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Cerberus.Api.Filters;

[AttributeUsage(AttributeTargets.All)]
public class AppExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<AppExceptionFilter> _logger;


    public AppExceptionFilter(ILogger<AppExceptionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        var msg = new
        {
            context.Exception.Message,
            ExceptionType = context.Exception.GetType().ToString()
        };

        var exceptionType = context.Exception.GetType().BaseType;
        if (exceptionType != null)
        {
            var typeExceptionName = exceptionType.Name;

            switch (typeExceptionName)
            {
                case nameof(DomainBaseException):
                {
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Result = new ObjectResult(msg);
                }
                    break;
                default:
                {
                    _logger.Log(LogLevel.Error, context.Exception.Message);
                    context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Result = new ObjectResult(new
                        {message = "Ha ocurrido un error interno en la aplicación"});
                }
                    break;
            }
        }
    }
}