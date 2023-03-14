using FluentMigrator;

namespace Architect.Migrator.Migrations;

[Migration(0000L)]
public class AddExtensions : Migration
{
    public override void Up()
    {
        Execute.Sql(@"CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";");
    }

    public override void Down()
    {
    }
}
