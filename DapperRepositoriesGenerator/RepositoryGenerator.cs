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
            { "Namespace", options.RepositoryInterfaceNamespace },
            { "EntitiesNamespace", options.EntitiesNamespace },
            { "TableName", table.TableName },
            { "IdColumnName", GetIdColumn(table) },
            { "IdParameterName", GetIdParameter(table) },
            { "IdTypeName", GetIdTypeName(table) },
            { "TableParameterName", GetTableParameter(table) }
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
            { "DeleteRequest", sqlGenerator.GenerateDelete(table) },
            { "IdColumnName", GetIdColumn(table) },
            { "IdParameterName", GetIdParameter(table) },
            { "IdTypeName", GetIdTypeName(table) },
            { "TableParameterName", GetTableParameter(table) }
        };

        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
    
    private string GetIdColumn(DbTable table)
        => table.ColumnNames.First();

    private string GetIdParameter(DbTable table)
    {
        var id = GetIdColumn(table);
        return id.Substring(0, 1).ToLower() + id.Substring(1);
    }

    private string GetIdTypeName(DbTable table)
        => table.Columns.First().typeName;

    private string GetTableParameter(DbTable table)
        => table.TableName.Substring(0, 1).ToLower() + table.TableName.Substring(1);
}