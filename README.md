# DapperRepositoriesGenerator

Code generation example:

```csharp
var tables = new[]
{
    new DbTable("User", ["Id", "CreationDate", "ModificationDate", "Username", "FullName", "Group", "Password"]),
    new DbTable("Group", ["Id", "CreationDate", "ModificationDate", "Name", "Description"]),
};

var basePath = @"<your_path>";
Directory.CreateDirectory(basePath);
        
var repositoriesInterfaceNamespace = "MyApp.Application.Interfaces.Database.Repositories";
var repositoriesNamespace = "MyApp.Infrastructure.Database.Repositories";
var servicesInterfaceNamespace = "MyApp.Application.Interfaces.Database.Services";
var servicesNamespace = "MyApp.Application.Database.Services";
var entitiesNamespace = "MyApp.Domain.Entities";

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
        interfacesPath: basePath,
        implementationsPath: basePath)
    .GenerateServices(
        interfacesPath: basePath,
        implementationsPath: basePath)
    .GenerateEntities(basePath)
    .GenerateSqlCreationScript(basePath);

await filesGenerator.GenerateFilesAsync(tables);
```

Tables generation from existing database:

```csharp
var generator = new DbTableGenerator(GetConnection);
var tables = generator.Generate("User", "Group");
```