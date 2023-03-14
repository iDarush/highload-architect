using Architect.Web.BLL.Services;
using Architect.Web.BLL.Services.Interfaces;
using Architect.Web.DAL.Repositories.Interfaces;
using Moq;

namespace Architect.Tests.Mocks.Services;

public class UserServiceMock
{
    public Mock<IUserRepository> UserRepositoryMock { get; }

    public UserServiceMock(Mock<IUserRepository>? userRepositoryMock = null)
    {
        UserRepositoryMock = userRepositoryMock ?? new();
    }

    public IUserService GetUserService() => new UserService(UserRepositoryMock.Object);
}
