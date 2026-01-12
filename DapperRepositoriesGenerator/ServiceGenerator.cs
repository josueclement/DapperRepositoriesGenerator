using System.IO;
using System.Linq;
using System.Text;
using Scriban.Runtime;

namespace DapperRepositoriesGenerator;

public class ServiceGenerator(ServiceGeneratorOptions options)
{
    public string GenerateServiceInterface(DbTable table)
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.IServiceTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "Namespace", options.ServicesInterfaceNamespace },
            { "EntitiesNamespace", options.EntitiesNamespace },
            { "TableName", table.TableName },
            { "IdParameterName", table.GetIdParameterName() },
            { "IdTypeName", table.GetIdTypeName() },
            { "TableParameterName", table.GetTableParameterName() }
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
            { "ServiceInterfaceNamespace", options.ServicesInterfaceNamespace },
            { "RepositoryInterfaceNamespace", options.RepositoriesInterfaceNamespace },
            { "Namespace", options.ServicesNamespace },
            { "TableName", table.TableName },
            { "IdParameterName", table.GetIdParameterName() },
            { "IdTypeName", table.GetIdTypeName() },
            { "TableParameterName", table.GetTableParameterName() }
        };

        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
}