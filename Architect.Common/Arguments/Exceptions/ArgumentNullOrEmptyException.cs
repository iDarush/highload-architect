namespace Architect.Common.Arguments.Exceptions;

public class ArgumentNullOrEmptyException : ArgumentNullException
{
    public ArgumentNullOrEmptyException(string? paramName)
        : base(paramName, message: "Parameter is null or empty")
    {
    }
}
