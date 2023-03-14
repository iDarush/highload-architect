using Architect.Web;
using Mapster;

namespace Architect.Tests.Setup;

public class GlobalSetupFixture : IDisposable
{
    public GlobalSetupFixture()
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(Startup).Assembly);
    }

    public void Dispose()
    {
    }
}
