using Architect.Web.DAL.Models;
using Architect.Web.DAL.Parameters;

namespace Architect.Web.DAL.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetById(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<User>> Search(UserSearchParameter parameters, CancellationToken cancellationToken);

    Task<Guid> Insert(User user, CancellationToken cancellationToken);
}
