namespace DapperRepositoriesGenerator;

public class EntityGeneratorOptions
{
    public string EntitiesNamespace { get; set; } = "MyApp.Domain.Entities";
    public bool GenerateNotifyProperties { get; set; } = true;
    public bool NotifyPropertiesWithFieldKeyword { get; set; } = true;
}