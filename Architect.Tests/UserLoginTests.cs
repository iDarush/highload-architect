using Architect.Tests.Mocks;
using Architect.Tests.Mocks.Controllers;
using Architect.Tests.Setup;
using Architect.Web.BLL.Exceptions;
using Architect.Web.DAL.Models;
using Architect.Web.Dto.Requests;
using Architect.Web.Dto.Responses;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Architect.Tests;

[Collection(nameof(GlobalSetupCollection))]
public class UserLoginTests : IClassFixture<AuthControllerMoq>
{
    private readonly Faker _faker = new();
    private readonly AuthControllerMoq _authController;

    public UserLoginTests(AuthControllerMoq authController)
    {
        _authController = authController;
    }

    [Fact]
    public async Task User_WithCorrectCredentials_ShouldBeLoggedId()
    {
        var correctPassword = _faker.Random.String2(8);
        var correctLogin = _faker.Random.Guid();

        _authController.AuthService.UserRepositoryMock
            .Setup(
                x => x.GetById(
                    It.Is<Guid>(id => id == correctLogin),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(() => new User());

        _authController.AuthService.PasswordHasherMock.WithVerifyHashedPassword(correctPassword);

        var controller = _authController.GetAuthController();
        var actor = () => controller.Login(new UserLoginRequest(correctLogin.ToString(), correctPassword), default);

        var response = await actor.Should().NotThrowAsync();
        response.Which.Result.Should().BeOfType<OkObjectResult>();

        var userResponse = response.Which.Result!.GetOkObjectResultContent<UserLoginResponse>();
        userResponse.Should().NotBeNull();
        userResponse!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task User_WithIncorrectLogin_ShouldReceiveError()
    {
        var incorrectLogin = _faker.Random.Guid();

        _authController.AuthService.UserRepositoryMock
            .Setup(
                x => x.GetById(
                    It.Is<Guid>(id => id == incorrectLogin),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(() => (User?)null);

        var controller = _authController.GetAuthController();
        var actor = () => controller.Login(new UserLoginRequest(incorrectLogin.ToString(), "U83838383i"), default);

        var error = await actor.Should().ThrowExactlyAsync<InvalidCredentialsException>();
        error.Which.ErrorState.Should().Be(InvalidCredentialsException.State.InvalidUserId);
    }

    [Fact]
    public async Task User_WithIncorrectPassword_ShouldReceiveError()
    {
        var incorrectPassword = _faker.Random.String2(8);
        var correctPassword = _faker.Random.String2(8);
        var correctLogin = _faker.Random.Guid();

        _authController.AuthService.UserRepositoryMock
            .Setup(
                x => x.GetById(
                    It.Is<Guid>(id => id == correctLogin),
                    It.IsAny<CancellationToken>()
                ))
            .ReturnsAsync(() => new User());

        _authController.AuthService.PasswordHasherMock.WithVerifyHashedPassword(correctPassword);

        var controller = _authController.GetAuthController();
        var actor = () => controller.Login(new UserLoginRequest(correctLogin.ToString(), incorrectPassword), default);

        var error = await actor.Should().ThrowExactlyAsync<InvalidCredentialsException>();
        error.Which.ErrorState.Should().Be(InvalidCredentialsException.State.InvalidPassword);
    }
}
