using System.Net;

namespace Pincred.Template.Domain.Entities.ResultPattern;

public abstract class BaseResult
{
    public HttpStatusCode StatusCode { get; private set; }
    public object? Data { get; private set; }
    public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode < 300;

    public BaseResult(HttpStatusCode statusCode, object? data = null)
    {
        StatusCode = statusCode;
        Data = data;
    }
}