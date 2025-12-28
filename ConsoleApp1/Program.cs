using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Bogus;
using DapperRepositoriesGenerator;
using Scriban;
using Scriban.Runtime;

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

public class TestClass
{
    public string ClassName { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        DbProviderFactories.RegisterFactory(
            "System.Data.SQLite",
            "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");
        
        // TestScriban();
        Generate();
        
        // await TestInsert();
        // await TestSelectAll();
        // await TestSelectById();
        // await TestUpdate();
        // await TestDelete();
    }

    static void TestScriban()
    {
        // var testClass = new TestClass() { ClassName = "Toto" };
        var template = Template.Parse("public class {{ ClassName }} { }");
        var context = new TemplateContext();
        var scriptObject = new ScriptObject();
        scriptObject.Add("ClassName", "Toto");
        context.PushGlobal(scriptObject);
        var result = template.Render(context);
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
        var sqlGenerator = new SqlGenerator(new SqlGeneratorOptions
        {
            ParameterPrefix = "@",
            Quote = "`"
        });
        var repositoryGenerator = new RepositoryGenerator(sqlGenerator, new RepositoryGeneratorOptions
        {
            RepositoryInterfaceNamespace =  "ConsoleApp1",
            RepositoryNamespace = "ConsoleApp1",
            EntitiesNamespace =  "ConsoleApp1"
        });
        var serviceGenerator = new ServiceGenerator(new ServiceGeneratorOptions
        {
            ServiceInterfaceNamespace = "ConsoleApp1",
            ServiceNamespace = "ConsoleApp1",
            RepositoryInterfaceNamespace = "ConsoleApp1",
            EntitiesNamespace = "ConsoleApp1"
        });
        
        Console.WriteLine($"SelectAll: {sqlGenerator.GenerateSelectAll(usersTable)}");
        Console.WriteLine($"SelectById: {sqlGenerator.GenerateSelectById(usersTable)}");
        Console.WriteLine($"Insert: {sqlGenerator.GenerateInsert(usersTable)}");
        Console.WriteLine($"Update: {sqlGenerator.GenerateUpdate(usersTable)}");
        Console.WriteLine($"Delete: {sqlGenerator.GenerateDelete(usersTable)}");
        Console.WriteLine("---Repository:");
        Console.WriteLine(repositoryGenerator.GenerateRepositoryGenericInterface());
        Console.WriteLine("---Repository:");
        Console.WriteLine(repositoryGenerator.GenerateRepositoryInterface(usersTable));
        Console.WriteLine("---Repository:");
        Console.WriteLine(repositoryGenerator.GenerateRepository(usersTable));
        Console.WriteLine("---Service:");
        Console.WriteLine(serviceGenerator.GenerateServiceGenericInterface());
        Console.WriteLine("---Service:");
        Console.WriteLine(serviceGenerator.GenerateServiceInterface(usersTable));
        Console.WriteLine("---Service:");
        Console.WriteLine(serviceGenerator.GenerateService(usersTable));
        Console.WriteLine("Create table:");
        Console.WriteLine(sqlGenerator.GenerateCreateTableScript(usersTable));
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

