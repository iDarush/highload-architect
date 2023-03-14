namespace Architect.Web.DAL.Models;

public record User
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public short Age { get; init; }

    public short Gender { get; init; }

    public string Hobby { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public string PasswordHash { get; init; } = string.Empty;
}
