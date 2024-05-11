using Finder.Data.Enums;
using Humanizer;

namespace Finder.Domain.Models;

public class ApiErrorResult(
    StatusCode statusCode
)
{
    public int StatusCode { get; init; } = (int) statusCode;

    public string Message { get; init; } = statusCode.ToString().Humanize();
}