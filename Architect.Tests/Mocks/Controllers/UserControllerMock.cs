using Architect.Tests.Mocks.Services;
using Architect.Web.Controllers;
using Architect.Web.DAL.Repositories.Interfaces;
using Moq;

namespace Architect.Tests.Mocks.Controllers;

public class UserControllerMock
{
    public Mock<IUserRepository> UserRepositoryMock { get; } = new();

    public AuthServiceMock AuthService { get; }

    public UserServiceMock UserService { get; }

    public UserControllerMock()
    {
        AuthService = new AuthServiceMock(UserRepositoryMock);
        UserService = new UserServiceMock(UserRepositoryMock);
    }

    public UserController GetUserController() => new(AuthService.GetAuthService(), UserService.GetUserService());
}
