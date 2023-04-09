namespace Architect.Common.Deconstruction;

public static class Enumerable
{
    public static void Deconstruct(
        this string[] collection,
        out string first,
        out string second)
    {
        first = collection is { Length: > 0 } ? collection[0] : string.Empty;
        second = collection is { Length: > 1 } ? collection[1] : string.Empty!;
    }
}
