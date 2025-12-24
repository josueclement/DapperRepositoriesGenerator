using System;
using System.Linq;

namespace DapperRepositoriesGenerator;

public class DbTable
{
    private readonly string _tableName;
    private readonly string[] _columnNames;

    public DbTable(string tableName, params string[] columnNames)
    {
        _tableName = tableName;
        _columnNames = columnNames;
    }

    public string GenerateSqlRequestCreateTable()
    {
        throw new NotImplementedException();
        // return $"CREATE TABLE {_tableName} ( {string.Join(", ", _columnNames)} )";
    }

    public string GenerateSqlRequestSelectById()
    {
        return $"SELECT {string.Join(", ", _columnNames)} FROM {_tableName} WHERE Id = @Id";
    }
    
    public string GenerateSqlRequestSelectAll()
    {
        return $"SELECT {string.Join(", ", _columnNames)} FROM {_tableName}";
    }

    public string GenerateSqlRequestInsert()
    {
        return $"INSERT INTO {_tableName} ({string.Join(", ", _columnNames)} ) VALUES ({string.Join(", ", _columnNames.Select(c => $"@{c}"))})";
    }

    public string GenerateSqlRequestUpdate()
    {
        return $"UPDATE {_tableName} SET {string.Join(", ", _columnNames.Select(c => $"{c} = @{c}"))} WHERE Id = @Id";
    }

    public string GenerateSqlRequestDelete()
    {
        return $"DELETE FROM {_tableName} WHERE Id = @Id";
    }

    public string GenerateRepository()
    {
        throw new NotImplementedException();
    }
    
    
}