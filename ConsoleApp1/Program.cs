using System;
using System.Data.Common;
using System.Threading.Tasks;
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
        await TestRepo();
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
    }

    static async Task TestRepo()
    {
        DbProviderFactories.RegisterFactory(
            "System.Data.SQLite",
            "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");

        await using var connection = DbProviderFactories.GetFactory("System.Data.SQLite").CreateConnection();
        connection.ConnectionString = "Data Source=users.db";
        await connection.OpenAsync();

        var user = new User();
        user.Id = Guid.NewGuid().ToString();
        user.CreationDate = DateTime.Now.ToString("O");
        user.ModificationDate = DateTime.Now.AddDays(-4).ToString("O");
        user.Username = "jimi";
        user.FullName = "Jimi Hendrix";
        user.Password = "H3yJo3";
        
        var repo = new UserRepository(connection);
        // var addRes = await repo.AddAsync(user);
        var users = await repo.GetAllAsync();
        
        await connection.CloseAsync();
    }
}

