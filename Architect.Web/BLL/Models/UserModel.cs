using System.Security.Claims;

namespace Architect.Web.BLL.Models;

public record UserModel(
    Guid Id,
    string FirstName,
    string LastName,
    short Age,
    Gender Gender,
    string Hobby,
    string City)
{
    public ClaimsIdentity ClaimsIdentity => new(
        new Claim[]
        {
            new(ClaimTypes.NameIdentifier, Id.ToString()),
            new(ClaimTypes.Name, FirstName),
            new(ClaimTypes.Surname, LastName),
            new(ClaimTypes.Gender, Gender.ToString()),
            new(ClaimTypes.StateOrProvince, City)
        });
}
