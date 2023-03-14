using Architect.Common.Arguments;
using Architect.Web.DAL.Models;
using Architect.Web.DAL.Repositories;
using Architect.Web.DAL.Repositories.Interfaces;
using Dapper;
using Npgsql;
using Npgsql.NameTranslation;

namespace Architect.Web.DAL;

public static class Postgres
{
    private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();

    public static IServiceCollection AddDatabase(this IServiceCollection services, string? connectionString)
    {
        connectionString.Ensure().NotNullOrEmpty();

        DefaultTypeMap.MatchNamesWithUnderscores = true;
        services.AddNpgsqlDataSource(
            connectionString!,
            connectionLifetime: ServiceLifetime.Scoped,
            dataSourceBuilderAction:
            builder =>
            {
                builder.DefaultNameTranslator = Translator;
                builder.MapComposite<User>("user_v1", Translator);
            });
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
