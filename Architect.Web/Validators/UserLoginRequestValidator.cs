using Architect.Web.Dto.Requests;
using FluentValidation;

namespace Architect.Web.Validators;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty()
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage(
                "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)");

        RuleFor(user => user.Password)
            .NotEmpty()
            .Length(8);
    }
}
