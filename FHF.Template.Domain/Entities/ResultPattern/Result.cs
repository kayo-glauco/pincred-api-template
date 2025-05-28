using System.Net;

namespace FHF.Template.Domain.Entities.ResultPattern;

public sealed class Result : BaseResult
{
    public Result(HttpStatusCode statusCode, object? data = null)
        : base(statusCode, data)
    {
    }

    // Success
    public static Result Ok(object? data = null) => new(HttpStatusCode.OK, data);
    public static Result Created(object? data = null) => new(HttpStatusCode.Created, data);
    public static Result NoContent() => new(HttpStatusCode.NoContent);

    // Fail
    public static Result BadRequest(object? data = null) => new(HttpStatusCode.BadRequest, data);
    public static Result NotFound(object? data = null) => new(HttpStatusCode.NotFound, data);
    public static Result UnprocessableEntity(object? data = null) => new(HttpStatusCode.UnprocessableEntity, data);
    public static Result Unauthorized(object? data = null) => new(HttpStatusCode.Unauthorized, data);
    public static Result Conflict(object? data = null) => new Result(HttpStatusCode.Conflict, data);
}