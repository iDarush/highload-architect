using Architect.Web.DAL.Models;
using Architect.Web.DAL.Parameters;
using Architect.Web.DAL.Repositories.Interfaces;
using Architect.Web.Extensions;
using Dapper;
using Npgsql;
using SqlKata;

namespace Architect.Web.DAL.Repositories;

public class UserRepository : DbRepository, IUserRepository
{
    public UserRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
    }

    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = Users.Select(
                "id",
                "first_name",
                "last_name",
                "age",
                "gender",
                "hobby",
                "city",
                "password_hash")
            .Where("id", id);

        var command = BuildCommand(query, cancellationToken);
        using var connection = await GetConnectionAsync();
        var response = await connection.QueryFirstOrDefaultAsync<User>(command);
        return response;
    }

    public async Task<IReadOnlyCollection<User>> Search(
        UserSearchParameter parameters,
        CancellationToken cancellationToken)
    {
        if (parameters.IsEmpty)
        {
            return Array.Empty<User>();
        }

        var query = Users
            .Select(
                "id",
                "first_name",
                "last_name",
                "age",
                "gender",
                "hobby",
                "city")
            .OrderBy("id");

        if (!string.IsNullOrEmpty(parameters.FirstNamePrefix))
        {
            query.WhereStarts(
                "first_name",
                parameters.FirstNamePrefix,
                caseSensitive: true);
        }

        if (!string.IsNullOrEmpty(parameters.LastNamePrefix))
        {
            query.WhereStarts(
                "last_name",
                parameters.LastNamePrefix,
                caseSensitive: true);
        }

        var command = BuildCommand(query, cancellationToken, timeout: CommandTimeout.Long);
        using var connection = await GetConnectionAsync();
        var response = await connection.QueryAsync<User>(command);
        return response.ToArray();
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

        using var connection = await GetConnectionAsync();
        var response = await connection.QueryFirstAsync<Guid>(command);
        return response;
    }

    private static Query Users => new("users");
}
