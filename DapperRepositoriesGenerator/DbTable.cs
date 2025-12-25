using System;
using System.Linq;

namespace DapperRepositoriesGenerator;

public class DbTable
{
    private readonly string _tableName;
    private readonly string[] _columnNames;
    private readonly string _parameterPrefix;
    private readonly string _columnsQuote;

    public DbTable(
        string tableName,
        string[] columnNames,
        string parameterPrefix = "@",
        string columnsQuote = "`")
    {
        _tableName = tableName;
        _columnNames = columnNames;
        _parameterPrefix = parameterPrefix;
        _columnsQuote = columnsQuote;
    }
    
    public string GetAllColumns()
        => string.Join(", ", _columnNames.Select(c => $"{_columnsQuote}{c}{_columnsQuote}"));

    public string GetIdColumn()
        => _columnNames.First();
    
    // public string GetColumnsWithoutId()
    //     => string.Join(", ", _columnNames.Skip(1));
    
    public string GetAllParameters()
        => string.Join(", ", _columnNames.Select(c => $"{_parameterPrefix}{c}"));
    
    // public string GetParametersWithoutId()
    //     => string.Join(", ", _columnNames.Skip(1).Select(c => $"{_parameterPrefix}{c}"));
    
    public string GetColumnsSets()
        => string.Join(", ", _columnNames.Skip(1).Select(c => $"{_columnsQuote}{c}{_columnsQuote} = {_parameterPrefix}{c}"));

    public string GetWhereId()
        => $"WHERE {_columnsQuote}{GetIdColumn()}{_columnsQuote} = {_parameterPrefix}{GetIdColumn()}";

    public string GenerateSqlRequestCreateTable()
    {
        throw new NotImplementedException();
        // return $"CREATE TABLE {_tableName} ( {string.Join(", ", _columnNames)} )";
    }
    
    public string GenerateSqlRequestSelectAll()
    {
        return $"SELECT {GetAllColumns()} FROM {_tableName}";
    }

    public string GenerateSqlRequestSelectById()
    {
        return $"SELECT {GetAllColumns()} FROM {_tableName} {GetWhereId()}";
    }

    public string GenerateSqlRequestInsert()
    {
        return $"INSERT INTO {_tableName} ({GetAllColumns()}) VALUES ({GetAllParameters()})";
    }

    public string GenerateSqlRequestUpdate()
    {
        return $"UPDATE {_tableName} SET {GetColumnsSets()} {GetWhereId()}";
    }

    public string GenerateSqlRequestDelete()
    {
        return $"DELETE FROM {_tableName} {GetWhereId()}";
    }

    public string GenerateRepository()
    {
        throw new NotImplementedException();
    }
    
    
}