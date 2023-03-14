namespace Architect.Common.Arguments;

public class Argument<TValue>
{
    public TValue Value { get; }

    public string? ParamName { get; }

    public Argument(TValue value, string? paramName)
    {
        Value = value;
        ParamName = paramName;
    }

    public static implicit operator TValue(Argument<TValue> argument) => argument.Value;
}
