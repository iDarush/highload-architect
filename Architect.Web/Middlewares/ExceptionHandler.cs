using System.Net.Mime;
using System.Text.Json;
using Architect.Web.BLL.Exceptions;
using Architect.Web.Dto.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Architect.Web.Middlewares;

public class ExceptionHandler
{
    private readonly ILogger _logger;
    private readonly JsonSerializerOptions _options;

    public ExceptionHandler(ILogger<ExceptionHandler> logger, IOptionsSnapshot<JsonOptions> options)
    {
        _logger = logger;
        _options = options.Value!.JsonSerializerOptions;
    }

    public async Task Handle(HttpContext context)
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

        object? details = null;
        string message = "Internal error occured";

        if (exceptionHandlerFeature?.Error is BaseBusinessException businessException)
        {
            details = businessException.Details;
            message = businessException.Message;
            context.Response.StatusCode = (int)businessException.StatusCode;
        }
        else
        {
            if (exceptionHandlerFeature?.Error != null)
            {
                _logger.LogError(exceptionHandlerFeature.Error, message);
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(
                new ErrorResponse(message, details),
                _options));
    }
}
