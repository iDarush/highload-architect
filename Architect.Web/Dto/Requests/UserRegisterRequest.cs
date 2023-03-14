namespace Architect.Web.Dto.Requests;

public record UserRegisterRequest(
    string FirstName,
    string SecondName,
    short Age,
    short Gender,
    string? Biography,
    string City,
    string Password)
{
    public static UserRegisterRequest Empty(string password = "") => new(
        FirstName: string.Empty,
        SecondName: string.Empty,
        Age: 0,
        Gender: (short)BLL.Models.Gender.Other,
        Biography: null,
        City: string.Empty,
        Password: password);
};
