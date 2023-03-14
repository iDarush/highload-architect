using Microsoft.AspNetCore.Mvc;

namespace Architect.Tests;

public static class AspNetExtensions
{
    public static T? GetOkObjectResultContent<T>(this ActionResult result)
    {
        var objectResult = (ObjectResult)result;
        return objectResult.Value is T response ? response : default;
    }

    public static T? GetNotFoundObjectResultContent<T>(this ActionResult result)
    {
        var objectResult = (NotFoundObjectResult)result;
        return objectResult.Value is T response ? response : default;
    }
}
