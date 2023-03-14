using Architect.Web.BLL.Models;
using Architect.Web.Dto.Requests;
using FluentValidation;

namespace Architect.Web.Validators;

public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(r => r.Age).GreaterThan((short)0);

        RuleFor(r => r.FirstName).NotEmpty();

        RuleFor(r => r.SecondName).NotEmpty();

        RuleFor(r => r.City).NotEmpty();

        RuleFor(r => r.Password).Password();

        Transform(r => r.Gender, gender => (Gender)gender)
            .IsInEnum();
    }
}
