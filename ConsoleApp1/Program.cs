using System;
using DapperRepositoriesGenerator;

namespace ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        var usersTable = new DbTable("User", ["Id", "CreationDate", "ModificationDate", "Username", "FullName", "Password"]);
        Console.WriteLine($"SelectAll: {usersTable.GenerateSqlRequestSelectAll()}");
        Console.WriteLine($"SelectAll: {usersTable.GenerateSqlRequestSelectById()}");
        Console.WriteLine($"SelectAll: {usersTable.GenerateSqlRequestInsert()}");
        Console.WriteLine($"SelectAll: {usersTable.GenerateSqlRequestUpdate()}");
        Console.WriteLine($"SelectAll: {usersTable.GenerateSqlRequestDelete()}");
    }
}