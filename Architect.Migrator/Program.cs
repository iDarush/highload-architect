using Architect.Common.Arguments;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Architect.Migrator;

public class Program
{
    private const string ConnectionVariableName = "CONNECTION_STRING";

    static void Main()
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionVariableName);
        connectionString.Ensure().NotNullOrEmpty();

        using var serviceProvider = CreateServices(connectionString!);
        using var scope = serviceProvider.CreateScope();
        UpdateDatabase(scope.ServiceProvider);
    }

    private static ServiceProvider CreateServices(string connectionString)
        => new ServiceCollection()
            .AddMigrator(connectionString)
            .BuildServiceProvider(false);

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}
