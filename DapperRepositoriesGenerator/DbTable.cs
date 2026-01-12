using System.Linq;

namespace DapperRepositoriesGenerator;

public class DbTable
{
    public DbTable(string tableName, string[] columnNames)
    {
        TableName = tableName;
        Columns = columnNames.Select(x => (x, "string")).ToArray();
    }

    public DbTable(string tableName, (string columnName, string typeName)[] columns)
    {
        TableName = tableName;
        Columns = columns.ToArray();
    }
    
    public string TableName { get; }
    // public List<(string columnName, string columnType)> Columns { get; }
    public (string columnName, string typeName)[] Columns { get; }
    public string[] ColumnNames => Columns.Select(col => col.columnName).ToArray();
}
