using Architect.Web.BLL.Models;

namespace Architect.Web.BLL.Services.Interfaces;

public interface IUserService
{
    Task<UserModel?> GetUser(Guid id, CancellationToken cancellationToken);
}
