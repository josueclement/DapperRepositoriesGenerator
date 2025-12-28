using System.IO;
using System.Text;
using Scriban.Runtime;

namespace DapperRepositoriesGenerator;

public class ServiceGenerator(ServiceGeneratorOptions options)
{
    public string GenerateServiceGenericInterface()
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.IServiceGenericTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "Namespace", options.ServiceInterfaceNamespace }
        };
        
        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
    
    public string GenerateServiceInterface(DbTable table)
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.IServiceTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "Namespace", options.ServiceInterfaceNamespace },
            { "EntitiesNamespace", options.EntitiesNamespace },
            { "TableName", table.TableName }
        };
        
        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
    
    public string GenerateService(DbTable table)
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.ServiceTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "EntitiesNamespace", options.EntitiesNamespace },
            { "ServiceInterfaceNamespace", options.ServiceInterfaceNamespace },
            { "RepositoryInterfaceNamespace", options.RepositoryInterfaceNamespace },
            { "Namespace", options.ServiceNamespace },
            { "TableName", table.TableName }
        };

        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
}