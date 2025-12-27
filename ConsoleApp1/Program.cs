using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Bogus;
using DapperRepositoriesGenerator;

namespace ConsoleApp1;

// CREATE TABLE `User`(
//     `Id` TEXT NOT NULL,
//     `CreationDate` TEXT,
//     `ModificationDate` TEXT,
//     `Username` TEXT,
//     `FullName` TEXT,
//     `Password` TEXT,
// PRIMARY KEY(`Id`)
//     );

class Program
{
    static async Task Main(string[] args)
    {
        DbProviderFactories.RegisterFactory(
            "System.Data.SQLite",
            "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");
        
        Generate();
        
        // await TestInsert();
        // await TestSelectAll();
        // await TestSelectById();
        // await TestUpdate();
        // await TestDelete();
    }

    static DbConnection CreateConnection()
    {
        var connection = DbProviderFactories.GetFactory("System.Data.SQLite").CreateConnection();

        if (connection is null)
            throw new InvalidOperationException("Cannot create connection");
        
        connection.ConnectionString = "Data Source=users.db";
        return connection;
    }

    static void Generate()
    {
        var usersTable = new DbTable("User", ["Id", "CreationDate", "ModificationDate", "Username", "FullName", "Password"]);
        Console.WriteLine($"SelectAll: {usersTable.GenerateSqlRequestSelectAll()}");
        Console.WriteLine($"SelectById: {usersTable.GenerateSqlRequestSelectById()}");
        Console.WriteLine($"Insert: {usersTable.GenerateSqlRequestInsert()}");
        Console.WriteLine($"Update: {usersTable.GenerateSqlRequestUpdate()}");
        Console.WriteLine($"Delete: {usersTable.GenerateSqlRequestDelete()}");
        Console.WriteLine("Repository:");
        Console.WriteLine(new RepositoryGenerator(usersTable).GenerateRepository());
        Console.WriteLine("Create table:");
        Console.WriteLine(usersTable.GenerateSqlRequestCreateTable());
    }

    static async Task TestSelectAll()
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        var repo = new UserRepository(connection);
        var users = await repo.GetAllAsync();
        await connection.CloseAsync();
    }

    static async Task TestSelectById()
    {
    }

    static async Task TestInsert()
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        var repo = new UserRepository(connection);
        
        var faker = new Faker<User>()
            .CustomInstantiator(f =>
            {
                var firstName = f.Name.FirstName();
                var lastName = f.Name.LastName();

                return new User()
                {
                    Id = Ulid.NewUlid().ToString(),
                    CreationDate = f.Date.Past(5).ToString("O"),
                    ModificationDate = f.Date.Past(1).ToString("O"),
                    Username = f.Internet.UserName(firstName, lastName),
                    FullName = firstName + " " + lastName,
                    Password = f.Internet.Email(firstName, lastName)
                };
            });

        var people = faker.Generate(10);

        foreach (var person in people)
        {
            await repo.AddAsync(person);
        }
        await connection.CloseAsync();
    }

    static async Task TestUpdate()
    {
        
    }

    static async Task TestDelete()
    {
        
    }
}

