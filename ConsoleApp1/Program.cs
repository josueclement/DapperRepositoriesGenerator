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
        // await GenerateEventManager();
        await GenerateEntities();
    }

    static async Task GenerateEntities()
    {
        var tables = new[]
        {
            new DbTable("Artist", ["Id", "CreationDate", "ModificationDate", "ParentId", "Name", "Role", "Hotel", "StayStartDate", "StayEndDate", "Contact", "TravelDetails", "Transport", "Parking", "Misc", "Contract", "Rider", "Menus", "Roadmap", "Suisa", "Remarks"])
        };

        var entityGenerator = new EntityGenerator(new EntityGeneratorOptions
        {
            GenerateNotifyProperties = true
        });
        var res = entityGenerator.GenerateEntity(tables.First());
        Console.WriteLine(res);
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
                //TODO: rename options with ies
                options.RepositoryInterfaceNamespace = repositoriesInterfaceNamespace;
                options.RepositoryNamespace = repositoriesNamespace;
                options.EntitiesNamespace = entitiesNamespace;
            })
            .SetServiceGeneratorOptions(options =>
            {
                options.ServiceInterfaceNamespace = servicesInterfaceNamespace;
                options.ServiceNamespace = servicesNamespace;
                options.RepositoryInterfaceNamespace = repositoriesInterfaceNamespace;
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