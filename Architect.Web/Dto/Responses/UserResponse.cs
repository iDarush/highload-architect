namespace Architect.Web.Dto.Responses;

public record UserResponse(
    string Id,
    string FirstName,
    string SecondName,
    short Age,
    short Gender,
    string Biography,
    string City);
