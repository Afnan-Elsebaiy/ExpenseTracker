using ExpenseTracker.Application.Common.Enums;

namespace ExpenseTracker.Application.Common.Response;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }

    public ResponseType ResponseType { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public List<string> Errors { get; set; } = [];
}