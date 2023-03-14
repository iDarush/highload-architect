using Architect.Tests.Mocks;
using Architect.Tests.Setup;
using Architect.Web.Validators;
using FluentAssertions;
using Xunit;

namespace Architect.Tests;

[Collection(nameof(GlobalSetupCollection))]
public class PasswordValidationTests
{
    [Theory]
    [MemberData(nameof(PasswordsData))]
    public void UserPassword_ShouldBeValidated(string password, bool correct)
    {
        var validator = new StringValidator();
        validator.RuleFor(v => v).Password();

        var validationResult = validator.Validate(password);
        validationResult.IsValid.Should().Be(correct);
    }

    public static IEnumerable<object[]> PasswordsData()
    {
        yield return new object[]
        {
            "U83838383i",
            true
        };

        yield return new object[]
        {
            "",
            false
        };

        yield return new object[]
        {
            "U8u",
            false
        };

        yield return new object[]
        {
            "UUUUUUUUUU8888888888uuuuuuuuuuu",
            false
        };

        yield return new object[]
        {
            "88888888",
            false
        };

        yield return new object[]
        {
            "UUUU8888",
            false
        };

        yield return new object[]
        {
            "uuuu8888",
            false
        };
    }
}
