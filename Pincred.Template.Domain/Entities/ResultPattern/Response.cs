namespace Pincred.Template.Domain.Entities.ResultPattern;

public sealed class Response
{
    public string Message { get; }
    public object? Details { get; }

    public Response(string message, object? details = null)
    {
        Message = message;
        Details = details;
    }
}