using Architect.Web.DAL.Models;
using Architect.Web.DAL.Repositories.Interfaces;
using Architect.Web.Dto.Requests;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Architect.Tests.Mocks;

public static class Extensions
{
    private static readonly Faker Faker = new();

    public static Guid WithInsert(this Mock<IUserRepository> repo)
    {
        var userId = Guid.NewGuid();

        repo.Setup(
                r => r.Insert(
                    It.IsAny<User>(),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(() => userId);

        return userId;
    }

    public static string WithHashPassword(this Mock<IPasswordHasher<UserRegisterRequest>> hasher)
    {
        var hash = Faker.Random.String2(10);

        hasher.Setup(
                x => x.HashPassword(
                    It.IsAny<UserRegisterRequest>(),
                    It.IsAny<string>()
                ))
            .Returns(() => hash);

        return hash;
    }

    public static void WithVerifyHashedPassword(
        this Mock<IPasswordHasher<UserRegisterRequest>> hasher,
        string correctPassword)
    {
        hasher.Setup(
                x => x.VerifyHashedPassword(
                    It.IsAny<UserRegisterRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()
                ))
            .Returns(
                (
                    UserRegisterRequest _,
                    string _,
                    string providedPassword) => providedPassword == correctPassword
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed);
    }
}
