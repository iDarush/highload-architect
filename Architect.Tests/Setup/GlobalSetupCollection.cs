using Xunit;

namespace Architect.Tests.Setup;

[CollectionDefinition(nameof(GlobalSetupCollection))]
public class GlobalSetupCollection : ICollectionFixture<GlobalSetupFixture>
{
}
