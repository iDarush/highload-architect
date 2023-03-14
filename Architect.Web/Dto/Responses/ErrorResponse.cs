namespace Architect.Web.Dto.Responses;

public record ErrorResponse(string Message, object? Details = null);
