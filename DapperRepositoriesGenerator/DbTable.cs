namespace DapperRepositoriesGenerator;

public class DbTable
{
    public DbTable(string tableName, string[] columnNames)
    {
        TableName = tableName;
        ColumnNames = columnNames;
    }
    
    public string TableName { get; }
    public string[] ColumnNames { get; }
}