using System;
using System.IO;
using System.Linq;
using System.Text;
using Scriban.Runtime;

namespace DapperRepositoriesGenerator;

public class RepositoryGenerator(
    SqlGenerator sqlGenerator,
    RepositoryGeneratorOptions options)
{
    public string GenerateRepositoryInterface(DbTable table)
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.IRepositoryTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "Namespace", options.RepositoriesInterfaceNamespace },
            { "EntitiesNamespace", options.EntitiesNamespace },
            { "TableName", table.TableName },
            { "IdColumnName", table.GetIdColumnName() },
            { "IdParameterName", table.GetIdParameterName() },
            { "IdTypeName", table.GetIdTypeName() },
            { "TableParameterName", table.GetTableParameterName() }
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
            { "RepositoryInterfaceNamespace", options.RepositoriesInterfaceNamespace },
            { "Namespace", options.RepositoriesNamespace },
            { "TableName", table.TableName },
            { "SelectAllRequest", sqlGenerator.GenerateSelectAll(table) },
            { "SelectByIdRequest", sqlGenerator.GenerateSelectById(table) },
            { "InsertRequest", sqlGenerator.GenerateInsert(table) },
            { "UpdateRequest", sqlGenerator.GenerateUpdate(table) },
            { "DeleteRequest", sqlGenerator.GenerateDelete(table) },
            { "IdColumnName", table.GetIdColumnName() },
            { "IdParameterName", table.GetIdParameterName() },
            { "IdTypeName", table.GetIdTypeName() },
            { "TableParameterName", table.GetTableParameterName() }
        };

        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
}