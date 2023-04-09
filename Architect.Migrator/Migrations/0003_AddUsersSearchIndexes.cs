using System.Globalization;
using FluentMigrator;
using System.Reflection;
using System.Text;
using Architect.Common.Deconstruction;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Primitives;

namespace Architect.Migrator.Migrations;

[Migration(0003L, TransactionBehavior.None)]
public class AddUsersSearchIndexes : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE INDEX IF NOT EXISTS users_first_name_last_name_idx
            ON users USING btree (first_name text_pattern_ops, last_name text_pattern_ops);
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP INDEX users_first_name_last_name_idx;
        ");
    }
}
