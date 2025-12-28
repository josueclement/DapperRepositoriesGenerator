using System;
using System.IO;
using System.Text;
using Scriban.Runtime;

namespace DapperRepositoriesGenerator;

public class RepositoryGenerator(
    SqlGenerator sqlGenerator,
    RepositoryGeneratorOptions options)
{
    public string GenerateRepositoryGenericInterface()
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.IRepositoryGenericTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "Namespace", options.RepositoryInterfaceNamespace }
        };
        
        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }

    public string GenerateRepositoryInterface(DbTable table)
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.IRepositoryTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "Namespace", options.RepositoryInterfaceNamespace },
            { "EntitiesNamespace", options.EntitiesNamespace },
            { "TableName", table.TableName }
        };
        
        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
    
    public string GenerateRepository(DbTable table)
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.RepositoryTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "EntitiesNamespace", options.EntitiesNamespace },
            { "RepositoryInterfaceNamespace", options.RepositoryInterfaceNamespace },
            { "Namespace", options.RepositoryNamespace },
            { "TableName", table.TableName },
            { "SelectAllRequest", sqlGenerator.GenerateSelectAll(table) },
            { "SelectByIdRequest", sqlGenerator.GenerateSelectById(table) },
            { "InsertRequest", sqlGenerator.GenerateInsert(table) },
            { "UpdateRequest", sqlGenerator.GenerateUpdate(table) },
            { "DeleteRequest", sqlGenerator.GenerateDelete(table) }
        };

        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
}