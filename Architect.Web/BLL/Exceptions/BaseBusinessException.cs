using System.Net;

namespace Architect.Web.BLL.Exceptions;

public abstract class BaseBusinessException : Exception
{
    public new abstract string Message { get; }

    public abstract object? Details { get; }

    public abstract HttpStatusCode StatusCode { get; }
}
