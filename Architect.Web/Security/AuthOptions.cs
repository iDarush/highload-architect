using Architect.Common.Arguments;

namespace Architect.Web.Security;

public class AuthOptions
{
    public string Issuer { get; set; } = string.Empty;

    public string SecretKey { get; set; } = string.Empty;

    public short TokenLifetimeInDays { get; set; }

    public void Validate()
    {
        Issuer.Ensure().NotNullOrEmpty();
        SecretKey.Ensure().NotNullOrEmpty();
    }
}
