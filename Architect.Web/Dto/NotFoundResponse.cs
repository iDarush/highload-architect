using Architect.Web.Dto.Responses;

namespace Architect.Web.Dto;

public static class NotFoundResponse
{
    public static ErrorResponse User(Guid id) => new("User not found", id);
}
