using FluentValidation;

namespace Architect.Web.Validators;

public static class PasswordValidator
{
    public static IRuleBuilderOptions<T, string> Password<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int minLength = 8,
        int maxLength = 20)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(minLength)
            .WithMessage($"Password must be at least {minLength} characters.")
            .MaximumLength(maxLength)
            .WithMessage($"Password cannot exceed {maxLength} characters.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one digit.");
    }
}
