using Architect.Tests.Mocks.Services;
using Architect.Web.Controllers;

namespace Architect.Tests.Mocks.Controllers;

public class AuthControllerMoq
{
    public AuthServiceMock AuthService { get; } = new();

    public AuthController GetAuthController() => new(AuthService.GetAuthService());
}
