using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Scriban.Runtime;

namespace DapperRepositoriesGenerator;

public class EntityGenerator(EntityGeneratorOptions options)
{
    public string GenerateEntity(DbTable table)
    {
        var templateResourceName = options.GenerateNotifyProperties
            ? "DapperRepositoriesGenerator.Templates.EntityNotifyField.txt"
            : "DapperRepositoriesGenerator.Templates.EntitySimple.txt";
        
        using var entityTemplateStream = AssemblyHelper.GetEmbeddedStream(templateResourceName);
        using var reader = new StreamReader(entityTemplateStream, Encoding.UTF8);
        var templateContent = reader.ReadToEnd();

        var scriptObject = new ScriptObject
        {
            { "Namespace", options.EntitiesNamespace },
            { "EntityName", table.TableName },
            { "Properties", table.Columns.Select(x => new Dictionary<string, object> 
                { 
                    { "Name", x.columnName },
                    { "TypeName", x.typeName } 
                }).ToList()
            }
        };

        return ScribanHelper.RenderTemplate(templateContent, scriptObject);
    }
}