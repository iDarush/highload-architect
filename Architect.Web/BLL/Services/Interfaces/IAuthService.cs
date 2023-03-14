using Architect.Web.Dto.Requests;

namespace Architect.Web.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<string> Login(
        Guid userId,
        string password,
        CancellationToken cancellationToken);

    Task<Guid> Register(
        UserRegisterRequest request,
        CancellationToken cancellationToken);
}
