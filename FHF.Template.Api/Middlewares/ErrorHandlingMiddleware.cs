using Serilog;
using System.Net;
using System.Text.Json;

namespace FHF.Template.Api.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            Log.Information(ex, "Error app - {Ex}", ex.Message);

            context.Response.ContentType = "application/json";
            string message = string.Empty;
            object? dados = null;

            if (ex is ArgumentNullException)
            {

            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                message = "Erro interno";
            }

            var result = new { mensagem = message, dados };

            await context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}