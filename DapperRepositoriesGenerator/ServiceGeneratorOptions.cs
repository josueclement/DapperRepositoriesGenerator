namespace DapperRepositoriesGenerator;

public class ServiceGeneratorOptions
{
    public string ServicesInterfaceNamespace { get; set; } = "MyApp.Application.Interfaces.Database.Services";
    public string RepositoriesInterfaceNamespace { get; set; } = "MyApp.Application.Interfaces.Database.Repositories";
    public string ServicesNamespace { get; set; } = "MyApp.Application.Database.Services";
    public string EntitiesNamespace { get; set; } = "MyApp.Domain.Entities";
}