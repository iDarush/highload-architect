using Architect.Web.DAL.Models;

namespace Architect.Web.DAL.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetById(Guid id, CancellationToken cancellationToken);

    Task<Guid> Insert(User user, CancellationToken cancellationToken);
}
