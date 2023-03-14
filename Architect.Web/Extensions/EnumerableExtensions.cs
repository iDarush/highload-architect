namespace Architect.Web.Extensions;

public static class EnumerableExtensions
{
    public static T[] MakeArray<T>(this T element) => new[]
    {
        element
    };
}
