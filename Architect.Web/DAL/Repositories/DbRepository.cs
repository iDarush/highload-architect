using System.Data;
using Dapper;
using Npgsql;
using SqlKata;
using SqlKata.Compilers;

namespace Architect.Web.DAL.Repositories;

public abstract class DbRepository
{
    protected static readonly Compiler Compiler = new PostgresCompiler();

    private readonly NpgsqlDataSource _dataSource;

    protected DbRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    protected async ValueTask<IDbConnection> GetConnectionAsync()
    {
        return await _dataSource.OpenConnectionAsync();
    }

    protected CommandDefinition BuildCommand(
        Query query,
        CancellationToken cancellationToken,
        int timeout = CommandTimeout.Medium)
    {
        var compiled = Compiler.Compile(query);

        var command = new CommandDefinition(
            commandText: compiled.Sql,
            parameters: compiled.NamedBindings,
            commandTimeout: timeout,
            cancellationToken: cancellationToken);

        return command;
    }
}
