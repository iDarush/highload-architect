using System.Runtime.CompilerServices;
using Architect.Common.Arguments.Exceptions;

namespace Architect.Common.Arguments;

public static class ArgumentExtensions
{
    public static Argument<T> Ensure<T>(
        this T argument,
        [CallerArgumentExpression("argument")] string? paramName = null)
    {
        return new Argument<T>(argument, paramName);
    }

    public static Argument<T> NotNull<T>(this Argument<T> argument)
    {
        return argument.Value != null ? argument : throw new ArgumentNullException(argument.ParamName);
    }

    public static Argument<string?> NotNullOrEmpty(this Argument<string?> argument)
    {
        return !string.IsNullOrEmpty(argument.Value) ? argument : throw new ArgumentNullOrEmptyException(argument.ParamName);
    }
}
