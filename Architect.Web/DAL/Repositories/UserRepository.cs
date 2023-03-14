using Architect.Web.DAL.Models;
using Architect.Web.DAL.Repositories.Interfaces;
using Architect.Web.Extensions;
using Dapper;
using Npgsql;

namespace Architect.Web.DAL.Repositories;

public class UserRepository : DbRepository, IUserRepository
{
    public UserRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
    }

    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT id, first_name, last_name, age, gender, hobby, city, password_hash FROM users
            WHERE id = @Id
            LIMIT 1";

        var param = new { Id = id };
        var command = new CommandDefinition(
            sql,
            param,
            commandTimeout: CommandTimeout.Medium,
            cancellationToken: cancellationToken);

        var connection = await GetConnectionAsync();
        var response = await connection.QueryFirstOrDefaultAsync<User>(command);
        return response;
    }

    public async Task<Guid> Insert(User user, CancellationToken cancellationToken)
    {
        const string sql = @"
            INSERT INTO users (first_name, last_name, age, gender, hobby, city, password_hash)
            SELECT first_name, last_name, age, gender, hobby, city, password_hash
            FROM UNNEST(@Users)
            RETURNING id";

        var param = new { Users = user.MakeArray() };
        var command = new CommandDefinition(
            sql,
            param,
            commandTimeout: CommandTimeout.Medium,
            cancellationToken: cancellationToken);

        var connection = await GetConnectionAsync();
        var response = await connection.QueryFirstAsync<Guid>(command);
        return response;
    }
}
