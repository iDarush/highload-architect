using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Architect.Migrator;

public static class MigratorExtensions
{
    public static IServiceCollection AddMigrator(this IServiceCollection services, string connectionString)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(
                rb => rb
                    .AddPostgres()
                    .WithVersionTable(new VersionTable())
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(VersionTable).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services;
    }
}
