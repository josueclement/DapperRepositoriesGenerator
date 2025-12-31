using System.Text;

namespace DapperRepositoriesGenerator;

public class EntityGenerator(EntityGeneratorOptions options)
{
    public string GenerateEntity(DbTable table)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("using System;");
        sb.AppendLine();
        sb.AppendLine($"namespace {options.EntitiesNamespace};");
        sb.AppendLine();
        sb.AppendLine($"public class {table.TableName}");
        sb.AppendLine("{");

        foreach (var columnName in table.ColumnNames)
            sb.AppendLine($"    public string? {columnName} {{ get; set; }}");
        
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}