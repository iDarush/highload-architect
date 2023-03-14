using Architect.Tests.Mocks.Controllers;
using Architect.Tests.Setup;
using Architect.Web.BLL.Models;
using Architect.Web.DAL.Models;
using Architect.Web.Dto.Responses;
using AutoBogus;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Architect.Tests;

[Collection(nameof(GlobalSetupCollection))]
public class UserQueryTests : IClassFixture<UserControllerMock>
{
    private static readonly Faker<User> UserGenerator = new AutoFaker<User>()
        .RuleFor(u => u.Age, faker => faker.Random.Short(1))
        .RuleFor(u => u.Gender, faker => (short)faker.PickRandom<Gender>());

    private readonly UserControllerMock _userController;

    public UserQueryTests(UserControllerMock userController)
    {
        _userController = userController;
    }

    [Fact]
    public async Task User_WhenExists_ShouldReturnsSuccessfully()
    {
        var userData = UserGenerator.Generate();
        SetupRepository(userData);

        var controller = _userController.GetUserController();

        var actor = () => controller.Get(userData.Id, default);

        var response = await actor.Should().NotThrowAsync();
        response.Which.Result.Should().BeOfType<OkObjectResult>();
        var userResponse = response.Which.Result!.GetOkObjectResultContent<UserResponse>();
        userResponse.Should().NotBeNull();
        AssertUser(userResponse!, userData).Should().Be(true);
    }

    [Fact]
    public async Task User_WhenNotExists_ShouldReturnsNotFound()
    {
        var request = Guid.NewGuid();
        var userData = UserGenerator.Generate();
        SetupRepository(userData);

        var controller = _userController.GetUserController();

        var actor = () => controller.Get(request, default);

        var response = await actor.Should().NotThrowAsync();
        response.Which.Result.Should().BeOfType<NotFoundObjectResult>();

        var userResponse = response.Which.Result!.GetNotFoundObjectResultContent<ErrorResponse>();
        userResponse.Should().NotBeNull();
        userResponse!.Details.Should().Be(request);
        userResponse!.Message.Should().Be("User not found");
    }

    private static bool AssertUser(
        UserResponse response,
        User saved)
    {
        return saved.Id.ToString() == response.Id
               && saved.Gender == response.Gender
               && saved.FirstName == response.FirstName
               && saved.LastName == response.SecondName
               && saved.City == response.City
               && saved.Age == response.Age
               && saved.Hobby == response.Biography;
    }

    private void SetupRepository(User userData)
    {
        _userController.UserRepositoryMock
            .Setup(
                r => r.GetById(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                ))
            .Returns(
                (Guid id, CancellationToken _) =>
                {
                    User? result = id == userData.Id
                        ? userData
                        : null;

                    return Task.FromResult(result);
                });
    }
}
