using Finder.Data.Enums;

namespace Finder.Domain.Exceptions;

public class ApiException(StatusCode statusCode) : Exception
{
    public StatusCode StatusCode { get; } = statusCode;
}