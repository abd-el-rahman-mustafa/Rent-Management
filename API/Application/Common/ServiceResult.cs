namespace API.Application.Common;

/// <summary>
/// Wraps a service operation result. On success it carries <typeparamref name="T"/> data;
/// on failure it carries a title, detail message, and HTTP status code.
/// </summary>
public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }

    public required string Title { get; set; }
    public required string Detail { get; set; }
    public int StatusCode { get; set; }


    public static ServiceResult<T> Success(T data, string title, string detail, int statusCode = StatusCodes.Status200OK) =>
        new() { IsSuccess = true, Data = data, StatusCode = statusCode, Title = title, Detail = detail };

    /// <summary>Creates a failure result with a title, detail, and HTTP status code.</summary>
    public static ServiceResult<T> Failure(string title, string detail, int statusCode) =>
        new() { IsSuccess = false, Title = title, Detail = detail, StatusCode = statusCode };

    // ── Common failure shortcuts ───────────────────────────────────────────────

    public static ServiceResult<T> NotFound(string detail) =>
        Failure("Not Found", detail, StatusCodes.Status404NotFound);

    public static ServiceResult<T> BadRequest(string detail) =>
        Failure("Bad Request", detail, StatusCodes.Status400BadRequest);

    public static ServiceResult<T> Unauthorized(string detail = "Unauthorized access.") =>
        Failure("Unauthorized", detail, StatusCodes.Status401Unauthorized);

    public static ServiceResult<T> Conflict(string detail) =>
        Failure("Conflict", detail, StatusCodes.Status409Conflict);

    public static ServiceResult<T> InternalError(string detail = "An unexpected error occurred.") =>
        Failure("Internal Server Error", detail, StatusCodes.Status500InternalServerError);
}
