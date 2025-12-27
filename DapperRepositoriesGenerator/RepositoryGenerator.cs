using System;
using System.IO;
using System.Text;
using Scriban.Runtime;

namespace DapperRepositoriesGenerator;

public class RepositoryGenerator(DbTable table, SqlGenerator sqlGenerator)
{
    public string GenerateRepository(string repositoryNamespace = "MyNamespace")
    {
        using var repositoryTemplateStream = AssemblyHelper.GetEmbeddedStream("DapperRepositoriesGenerator.Templates.RepositoryTemplate.txt");
        using var reader = new StreamReader(repositoryTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject();
        scriptObject.Add("RepositoryNamespace", repositoryNamespace);
        scriptObject.Add("RepositoryName", "");
        scriptObject.Add("TableName", table.TableName);
        scriptObject.Add("SelectAllRequest", sqlGenerator.GenerateSelectAll());
        scriptObject.Add("SelectByIdRequest", sqlGenerator.GenerateSelectById());
        scriptObject.Add("InsertRequest", sqlGenerator.GenerateInsert());
        scriptObject.Add("UpdateRequest", sqlGenerator.GenerateUpdate());
        scriptObject.Add("DeleteRequest", sqlGenerator.GenerateDelete());
        
        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
}