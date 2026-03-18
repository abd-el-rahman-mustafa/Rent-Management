namespace API.Application.Common;

/// <summary>
/// Wraps a service operation result. On success it carries <typeparamref name="T"/> data;
/// on failure it carries a title, details message, and HTTP status code.
/// </summary>
public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }

    public required string Title { get; set; }
    public required string Details { get; set; }
    public int statusCode { get; set; }


    public static ServiceResult<T> Success(T data, string title, string details, int statusCode = StatusCodes.Status200OK) =>
        new() { IsSuccess = true, Data = data, statusCode = statusCode, Title = title, Details = details };

    /// <summary>Creates a failure result with a title, details, and HTTP status code.</summary>
    public static ServiceResult<T> Failure(string title, string details, int statusCode) =>
        new() { IsSuccess = false, Title = title, Details = details, statusCode = statusCode };

    // ── Common failure shortcuts ───────────────────────────────────────────────

    public static ServiceResult<T> NotFound(string details) =>
        Failure("Not Found", details, StatusCodes.Status404NotFound);

    public static ServiceResult<T> BadRequest(string details) =>
        Failure("Bad Request", details, StatusCodes.Status400BadRequest);

    public static ServiceResult<T> Unauthorized(string details = "Unauthorized access.") =>
        Failure("Unauthorized", details, StatusCodes.Status401Unauthorized);

    public static ServiceResult<T> Conflict(string details) =>
        Failure("Conflict", details, StatusCodes.Status409Conflict);

    public static ServiceResult<T> InternalError(string details = "An unexpected error occurred.") =>
        Failure("Internal Server Error", details, StatusCodes.Status500InternalServerError);
}
