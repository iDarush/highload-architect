using Architect.Web.BLL.Models;
using Architect.Web.BLL.Services.Interfaces;
using Architect.Web.DAL.Repositories.Interfaces;
using Mapster;

namespace Architect.Web.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserModel?> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(id, cancellationToken);
        return user?.Adapt<UserModel?>();
    }
}
