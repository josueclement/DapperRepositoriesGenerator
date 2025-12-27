using System;
using System.Linq;
using System.Text;

namespace DapperRepositoriesGenerator;

public class SqlGenerator(
    DbTable table,
    string parameterPrefix = "@",
    string quote = "`")
{
    public string TableName => table.TableName;
    public string[] ColumnNames => table.ColumnNames;
    
    public string GenerateCreateTableScript()
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"CREATE TABLE {quote}{TableName}{quote}(");
        
        foreach (var columnName in ColumnNames)
            sb.AppendLine($"    {quote}{columnName}{quote} TEXT,");
        
        sb.AppendLine($"    PRIMARY KEY({quote}{GetIdColumn()}{quote})");
        sb.AppendLine(");");
        
        return sb.ToString();
    }

    public string GenerateSelectAll()
        => $"SELECT {GetQuotedColumns()} FROM {quote}{TableName}{quote}";

    public string GenerateSelectById()
        => $"SELECT {GetQuotedColumns()} FROM {quote}{TableName}{quote} {GetWhereId()}";
    
    public string GenerateInsert()
        => $"INSERT INTO {quote}{TableName}{quote} ({GetQuotedColumns()}) VALUES ({GetParameters()})";
    
    public string GenerateUpdate()
        => $"UPDATE {quote}{TableName}{quote} SET {GetColumnsSets()} {GetWhereId()}";

    public string GenerateDelete()
        => $"DELETE FROM {quote}{TableName}{quote} {GetWhereId()}";

    private string GetQuotedColumns()
        => string.Join(", ", ColumnNames.Select(c => $"{quote}{c}{quote}"));
    
    private string GetIdColumn()
        => ColumnNames.First();
    
    private string GetParameters()
        => string.Join(", ", ColumnNames.Select(c => $"{parameterPrefix}{c}"));
    
    private string GetColumnsSets()
        => string.Join(", ", ColumnNames.Skip(1).Select(c => $"{quote}{c}{quote} = {parameterPrefix}{c}"));
    
    private string GetWhereId()
        => $"WHERE {quote}{GetIdColumn()}{quote} = {parameterPrefix}{GetIdColumn()}";
}