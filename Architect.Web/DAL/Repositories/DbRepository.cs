using System.Data;
using Npgsql;

namespace Architect.Web.DAL.Repositories;

public abstract class DbRepository
{
    private readonly NpgsqlDataSource _dataSource;

    protected DbRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    protected async ValueTask<IDbConnection> GetConnectionAsync()
    {
        return await _dataSource.OpenConnectionAsync();
    }
}
