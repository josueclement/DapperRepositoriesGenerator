using System;
using DapperRepositoriesGenerator;

namespace ConsoleApp1;

class Program
{
    static void Main(string[] args)
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
}