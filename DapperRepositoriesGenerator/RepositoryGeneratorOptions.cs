namespace DapperRepositoriesGenerator;

public class RepositoryGeneratorOptions
{
    public string RepositoriesInterfaceNamespace { get; set; } = "MyApp.Application.Interfaces.Database.Repositories";
    public string RepositoriesNamespace { get; set; } = "MyApp.Infrastructure.Database.Repositories";
    public string EntitiesNamespace { get; set; } = "MyApp.Domain.Entities";
}