namespace DapperRepositoriesGenerator;

public class ServiceGeneratorOptions
{
    public string ServiceInterfaceNamespace { get; set; } = "MyApp.Application.Interfaces.Database.Services";
    public string RepositoryInterfaceNamespace { get; set; } = "MyApp.Application.Interfaces.Database.Repositories";
    public string ServiceNamespace { get; set; } = "MyApp.Application.Databas.Database.Services";
    public string EntitiesNamespace { get; set; } = "MyApp.Domain.Entities";
}