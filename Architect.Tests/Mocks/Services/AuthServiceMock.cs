using Architect.Web.BLL.Services;
using Architect.Web.BLL.Services.Interfaces;
using Architect.Web.DAL.Repositories.Interfaces;
using Architect.Web.Dto.Requests;
using Architect.Web.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace Architect.Tests.Mocks.Services;

public class AuthServiceMock
{
    public AuthServiceMock(Mock<IUserRepository>? userRepositoryMock = null)
    {
        UserRepositoryMock = userRepositoryMock ?? new();
        AuthOptionsMock
            .Setup(x => x.Value)
            .Returns(
                new AuthOptions
                {
                    Issuer = "test",
                    TokenLifetimeInDays = 1,
                    SecretKey = "1111111111111111111111111111111"
                });
    }

    public Mock<IUserRepository> UserRepositoryMock { get; }

    public Mock<IPasswordHasher<UserRegisterRequest>> PasswordHasherMock { get; } = new();

    public Mock<IOptionsSnapshot<AuthOptions>> AuthOptionsMock { get; } = new();

    public IAuthService GetAuthService() => new AuthService(
        UserRepositoryMock.Object,
        AuthOptionsMock.Object,
        PasswordHasherMock.Object);
}
