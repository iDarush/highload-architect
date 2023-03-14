using System.Net;

namespace Architect.Web.BLL.Exceptions;

public class InvalidCredentialsException : BaseBusinessException
{
    public enum State
    {
        InvalidUserId = 1,
        InvalidPassword = 2,
        Other = 3
    }

    public InvalidCredentialsException(State errorState)
    {
        ErrorState = errorState;
    }

    public State ErrorState { get; }

    public override string Message => "Неверная пара ID/Пароль";

    public override object? Details => null;

    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
