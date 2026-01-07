namespace DapperRepositoriesGenerator;

public class RepositoryGeneratorOptions
{
    public string RepositoryInterfaceNamespace { get; set; } = "MyApp.Application.Interfaces.Database.Repositories";
    public string RepositoryNamespace { get; set; } = "MyApp.Infrastructure.Database.Repositories";
    public string EntitiesNamespace { get; set; } = "MyApp.Domain.Entities";
}