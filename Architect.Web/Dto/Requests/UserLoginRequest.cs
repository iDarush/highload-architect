namespace Architect.Web.Dto.Requests;

public record UserLoginRequest(string Id, string Password)
{
    public Guid GetIdAsGuid() => Guid.Parse(Id);
}
