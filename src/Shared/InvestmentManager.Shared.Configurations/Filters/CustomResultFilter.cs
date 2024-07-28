using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using InvestmentManager.Shared.Utilities.Abstractions.Result;

namespace InvestmentManager.Shared.Configurations.Filters;

/// <summary>
/// Filter to return responses value and status code according result patern implementation
/// </summary>
public class CustomResultFilter : IAsyncResultFilter
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;

        if (context.Result is not ObjectResult { Value: Result result })
        {
            await next();
            return;
        }

        if (result.IsSuccess)
        {
            context.HttpContext.Response.StatusCode = result.StatusCode;
            await WriteBodyWithResultValueAsync(result, context.HttpContext.Response);
            return;
        }

        context.HttpContext.Response.StatusCode = result.Error.StatusCode;
        await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(result.Error, _serializerOptions));
    }

    private async Task WriteBodyWithResultValueAsync(Result result, HttpResponse response)
    {
        if (result.StatusCode is (int)HttpStatusCode.NoContent)
        {
            return;
        }

        var resultValue = result.GetType().GetProperty("Value")?.GetValue(result);

        if (resultValue is not null)
        {
            await response.WriteAsync(JsonSerializer.Serialize(resultValue, _serializerOptions));
        }
    }
}
