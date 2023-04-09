namespace Architect.Web.DAL.Parameters;

public record UserSearchParameter(string FirstNamePrefix = "", string LastNamePrefix = "")
{
    public bool IsEmpty => string.IsNullOrWhiteSpace(FirstNamePrefix) &&
                           string.IsNullOrWhiteSpace(LastNamePrefix);
}
