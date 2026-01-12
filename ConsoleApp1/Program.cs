using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using DapperRepositoriesGenerator;
using Scriban;
using Scriban.Runtime;

namespace ConsoleApp1;

class Program
{
    static async Task Main(string[] args)
    {
        DbProviderFactories.RegisterFactory(
            "System.Data.SQLite",
            "System.Data.SQLite.SQLiteFactory, System.Data.SQLite");
        // await GenerateEventManager();
        await GenerateUsers();
        GenerateTables();
    }
    
    static DbConnection GetConnection()
    {
        var connection = DbProviderFactories.GetFactory("System.Data.SQLite").CreateConnection()
                         ?? throw new InvalidOperationException("Could not create database connection.");
        connection.ConnectionString = @"Data Source=C:\Temp\TestDB\test.sqlite";
        return connection;
    }

    static void GenerateTables()
    {
        var generator = new DbTableGenerator(GetConnection);
        var tables = generator.Generate("User", "Group");

    }

    static async Task GenerateUsers()
    {
        var generator = new DbTableGenerator(GetConnection);
        var tables = generator.Generate("User", "Group").ToArray();
        
        // var tables = new[]
        // {
        //     new DbTable("User", ["Id", "CreationDate", "ModificationDate", "Username", "FullName", "Group", "Password"]),
        //     new DbTable("Group", ["Id", "CreationDate", "ModificationDate", "Name", "Description"]),
        // };
        
        // var repositoriesInterfaceNamespace = "EventManager.Application.Interfaces.Repositories";
        // var repositoriesNamespace = "EventManager.Infrastructure.Database.Repositories";
        // var servicesInterfaceNamespace = "EventManager.Application.Interfaces.Services";
        // var servicesNamespace = "EventManager.Infrastructure.Database.Services";
        // var entitiesNamespace = "EventManager.Domain.Entities";

        var basePath = @"C:\Dev\JCL\DapperRepositoriesGenerator\ConsoleApp1\Generated";//"/home/jo/Dev/DapperRepositoriesGenerator/ConsoleApp1/Generated/";
        Directory.CreateDirectory(basePath);

        var filesGenerator = new FilesGenerator()
            .SetSqlGeneratorOptions(options => { })
            .SetRepositoryGeneratorOptions(options => { })
            .SetServiceGeneratorOptions(options => { })
            .SetEntityGeneratorOptions(options => { })
            .GenerateRepositories(
                interfacesPath: basePath,
                implementationsPath: basePath)
            .GenerateServices(
                interfacesPath: basePath,
                implementationsPath: basePath)
            .GenerateEntities(basePath)
            .GenerateSqlCreationScript(basePath);

        await filesGenerator.GenerateFilesAsync(tables);
    }

    static async Task GenerateEventManager()
    {
        var tables = new[]
        {
            new DbTable("Artist", ["Id", "CreationDate", "ModificationDate", "ParentId", "Name", "Role", "Hotel", "StayStartDate", "StayEndDate", "Contact", "TravelDetails", "Transport", "Parking", "Misc", "Contract", "Rider", "Menus", "Roadmap", "Suisa", "Remarks"]),
            new DbTable("Car", ["Id", "CreationDate", "ModificationDate", "Name", "Description", "LicensePlate"]),
            new DbTable("Location", ["Id", "CreationDate", "ModificationDate", "Name", "Description"]),
            new DbTable("Staff", ["Id", "CreationDate", "ModificationDate", "Name", "Contact", "Description"]),
            new DbTable("Volunteer", ["Id", "CreationDate", "ModificationDate", "Name", "Contact", "Description"])
        };
        
        var repositoriesInterfaceNamespace = "EventManager.Application.Interfaces.Repositories";
        var repositoriesNamespace = "EventManager.Infrastructure.Database.Repositories";
        var servicesInterfaceNamespace = "EventManager.Application.Interfaces.Services";
        var servicesNamespace = "EventManager.Infrastructure.Database.Services";
        var entitiesNamespace = "EventManager.Domain.Entities";

        var basePath = "/home/jo/Dev/EventManager/";

        var filesGenerator = new FilesGenerator()
            .SetSqlGeneratorOptions(options =>
            {
                options.ParameterPrefix = "@";
                options.Quote = "`";
            })
            .SetRepositoryGeneratorOptions(options =>
            {
                options.RepositoriesInterfaceNamespace = repositoriesInterfaceNamespace;
                options.RepositoriesNamespace = repositoriesNamespace;
                options.EntitiesNamespace = entitiesNamespace;
            })
            .SetServiceGeneratorOptions(options =>
            {
                options.ServicesInterfaceNamespace = servicesInterfaceNamespace;
                options.ServicesNamespace = servicesNamespace;
                options.RepositoriesInterfaceNamespace = repositoriesInterfaceNamespace;
                options.EntitiesNamespace = entitiesNamespace;
            })
            .SetEntityGeneratorOptions(options =>
            {
                options.EntitiesNamespace = entitiesNamespace;
            })
            .GenerateRepositories(
                interfacesPath: Path.Combine(basePath, "EventManager.Application", "Interfaces", "Repositories"),
                implementationsPath: Path.Combine(basePath, "EventManager.Infrastructure", "Database", "Repositories"))
            .GenerateServices(
                interfacesPath: Path.Combine(basePath, "EventManager.Application", "Interfaces", "Services"),
                implementationsPath: Path.Combine(basePath, "EventManager.Infrastructure", "Database", "Services"))
            .GenerateEntities(Path.Combine(basePath, "EventManager.Domain", "Entities"))
            .GenerateSqlCreationScript(Path.Combine(basePath, "EventManager.Infrastructure", "Database"));

        await filesGenerator.GenerateFilesAsync(tables);
    }
}