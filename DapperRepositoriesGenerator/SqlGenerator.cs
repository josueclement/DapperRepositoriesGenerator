using System;
using System.Linq;
using System.Text;

namespace DapperRepositoriesGenerator;

public class SqlGenerator(SqlGeneratorOptions options)
{
    public string GenerateCreateTableScript(DbTable table)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"CREATE TABLE {options.Quote}{table.TableName}{options.Quote}(");
        
        foreach (var columnName in table.ColumnNames)
            sb.AppendLine($"    {options.Quote}{columnName}{options.Quote} TEXT,");
        
        sb.AppendLine($"    PRIMARY KEY({options.Quote}{GetIdColumn(table)}{options.Quote})");
        sb.AppendLine(");");
        
        return sb.ToString();
    }

    public string GenerateSelectAll(DbTable table)
        => $"SELECT {GetQuotedColumns(table)} FROM {options.Quote}{table.TableName}{options.Quote}";

    public string GenerateSelectById(DbTable table)
        => $"SELECT {GetQuotedColumns(table)} FROM {options.Quote}{table.TableName}{options.Quote} {GetWhereId(table)}";
    
    public string GenerateInsert(DbTable table)
        => $"INSERT INTO {options.Quote}{table.TableName}{options.Quote} ({GetQuotedColumns(table)}) VALUES ({GetParameters(table)})";
    
    public string GenerateUpdate(DbTable table)
        => $"UPDATE {options.Quote}{table.TableName}{options.Quote} SET {GetColumnsSets(table)} {GetWhereId(table)}";

    public string GenerateDelete(DbTable table)
        => $"DELETE FROM {options.Quote}{table.TableName}{options.Quote} {GetWhereId(table)}";

    private string GetQuotedColumns(DbTable table)
        => string.Join(", ", table.ColumnNames.Select(c => $"{options.Quote}{c}{options.Quote}"));
    
    private string GetIdColumn(DbTable table)
        => table.ColumnNames.First();
    
    private string GetParameters(DbTable table)
        => string.Join(", ", table.ColumnNames.Select(c => $"{options.ParameterPrefix}{c}"));
    
    private string GetColumnsSets(DbTable table)
        => string.Join(", ", table.ColumnNames.Skip(1).Select(c => $"{options.Quote}{c}{options.Quote} = {options.ParameterPrefix}{c}"));
    
    private string GetWhereId(DbTable table)
        => $"WHERE {options.Quote}{GetIdColumn(table)}{options.Quote} = {options.ParameterPrefix}{GetIdColumn(table)}";
}