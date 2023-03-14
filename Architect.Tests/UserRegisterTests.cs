using Architect.Tests.Mocks;
using Architect.Tests.Mocks.Controllers;
using Architect.Tests.Setup;
using Architect.Web.BLL.Models;
using Architect.Web.DAL.Models;
using Architect.Web.Dto.Requests;
using Architect.Web.Dto.Responses;
using Architect.Web.Validators;
using AutoBogus;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Architect.Tests;

[Collection(nameof(GlobalSetupCollection))]
public class UserRegisterTests : IClassFixture<UserControllerMock>
{
    private readonly UserControllerMock _userController;

    public UserRegisterTests(UserControllerMock userController)
    {
        _userController = userController;
    }

    private static readonly Faker<UserRegisterRequest> UserRegisterRequestFaker = new AutoFaker<UserRegisterRequest>()
        .RuleFor(r => r.Gender, faker => (short)faker.PickRandom<Gender>()) // correct default Gender
        .RuleFor(r => r.Age, () => (short)1) // correct default Age
        .RuleFor(r => r.Password, () => "U83838383i"); // correct default strong Password

    [Theory]
    [MemberData(nameof(InvalidRequestsData))]
    public void RegisterRequest_ShouldBeValidated(UserRegisterRequest request, string errorField)
    {
        var validator = new UserRegisterRequestValidator();
        var validationResult = validator.Validate(request);

        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == errorField);
    }

    [Fact]
    public async Task User_ShouldBeRegistered()
    {
        var request = UserRegisterRequestFaker.Generate();
        var controller = _userController.GetUserController();
        var userId = _userController.UserRepositoryMock.WithInsert();
        var passwordHash = _userController.AuthService.PasswordHasherMock.WithHashPassword();

        var actor = () => controller.Register(request, default);

        var response = await actor.Should().NotThrowAsync();
        response.Which.Result.Should().BeOfType<OkObjectResult>();

        var registerResponse = response.Which.Result!.GetOkObjectResultContent<UserRegisterResponse>();
        registerResponse.Should().NotBeNull();
        registerResponse!.UserId.Should().Be(userId.ToString());

        _userController.UserRepositoryMock.Verify(
            x => x.Insert(
                It.Is<User>(user => AssertUser(passwordHash, request, user)),
                It.IsAny<CancellationToken>()
            ));
    }

    private static bool AssertUser(
        string passwordHash,
        UserRegisterRequest request,
        User saved)
    {
        return saved.PasswordHash == passwordHash
               && saved.Gender == request.Gender
               && saved.FirstName == request.FirstName
               && saved.LastName == request.SecondName
               && saved.City == request.City
               && saved.Age == request.Age
               && saved.Hobby == (request.Biography ?? string.Empty);
    }

    #region Theory Data

    public static IEnumerable<object[]> InvalidRequestsData()
    {
        yield return new object[]
        {
            UserRegisterRequestFaker.Generate() with { FirstName = string.Empty },
            nameof(UserRegisterRequest.FirstName)
        };

        yield return new object[]
        {
            UserRegisterRequestFaker.Generate() with { SecondName = string.Empty },
            nameof(UserRegisterRequest.SecondName)
        };

        yield return new object[]
        {
            UserRegisterRequestFaker.Generate() with { Age = 0 },
            nameof(UserRegisterRequest.Age)
        };

        yield return new object[]
        {
            UserRegisterRequestFaker.Generate() with { City = string.Empty },
            nameof(UserRegisterRequest.City)
        };

        yield return new object[]
        {
            UserRegisterRequestFaker.Generate() with { Gender = 99 },
            nameof(UserRegisterRequest.Gender)
        };
    }

    #endregion
}
