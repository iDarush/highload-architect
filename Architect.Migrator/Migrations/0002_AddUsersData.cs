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

[Migration(0002L)]
public class AddUsersData : Migration
{
    private static readonly CsvConfiguration CsvConfiguration =
        new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ","
        };

    public override void Up()
    {
        var executionPath = Assembly.GetExecutingAssembly().Location;
        var dataFilePath = Path.Join(
            Path.GetDirectoryName(executionPath),
            "Data",
            "people.csv");

        var userInsertion = Insert.IntoTable("users");

        using (var reader = new StreamReader(dataFilePath))
        using (var csv = new CsvReader(reader, CsvConfiguration))
        {
            var records = csv.GetRecords<UserLine>();

            foreach (var user in records)
            {
                var (firstName, lastName) = user.GetNames();

                userInsertion.Row(
                    new Dictionary<string, object>
                    {
                        ["first_name"] = firstName,
                        ["last_name"] = lastName,
                        ["age"] = user.Age,
                        ["gender"] = user.Gender,
                        ["hobby"] = user.Hobby,
                        ["city"] = user.City,
                        ["password_hash"] = user.Password
                    });
            }
        }
    }

    public override void Down()
    {
    }

    internal class UserLine
    {
        private static IPasswordHasher<UserLine> Hasher = new PasswordHasher<UserLine>();
        private static string DefaultHash = Hasher.HashPassword(new UserLine(), "a8838383Pi");

        [Index(0)]
        public string FullName { get; set; }

        [Index(1)]
        public short Age { get; set; }

        [Index(2)]
        public string City { get; set; }

        public short Gender => 3;

        public string Hobby => string.Empty;

        public string Password => DefaultHash;

        public (string FirstName, string LastName) GetNames()
        {
            var (firstName, lastName) = FullName.Split(' ');
            return (firstName, lastName);
        }
    }
}
