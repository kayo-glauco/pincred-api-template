using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pincred.Template.Domain.Entities.ResultPattern;

namespace Pincred.Template.Api.Filters;

public class ResultFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is Result result)
        {
            context.Result = ConvertToActionResult(result);
        }

        await next();
    }

    private static IActionResult ConvertToActionResult(Result result)
    {
        var response = new
        {
            statusCode = (int)result.StatusCode,
            data = result.Data,
            isSuccess = result.IsSuccess
        };

        return result.StatusCode switch
        {
            HttpStatusCode.OK => new OkObjectResult(response),
            HttpStatusCode.Created => new CreatedResult(string.Empty, response),
            HttpStatusCode.NoContent => new NoContentResult(),
            HttpStatusCode.BadRequest => new BadRequestObjectResult(response),
            HttpStatusCode.NotFound => new NotFoundObjectResult(response),
            HttpStatusCode.UnprocessableEntity => new UnprocessableEntityObjectResult(response),
            HttpStatusCode.Unauthorized => new UnauthorizedObjectResult(response),
            HttpStatusCode.Conflict => new ConflictObjectResult(response),
            _ => new ObjectResult(response) { StatusCode = (int)result.StatusCode }
        };
    }
}