using System;
using System.Linq;
using System.Text;

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
    
    public string TableName => _tableName;
    public string[] ColumnNames => _columnNames;
    
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
        var sb = new StringBuilder();
        sb.AppendLine($"CREATE TABLE {_columnsQuote}{_tableName}{_columnsQuote}(");
        foreach (var columnName in _columnNames)
            sb.AppendLine($"    {_columnsQuote}{columnName}{_columnsQuote} TEXT,");
        sb.AppendLine($"    PRIMARY KEY({_columnsQuote}{GetIdColumn()}{_columnsQuote})");
        sb.AppendLine(");");
        return sb.ToString();
    }

    // TODO: Generate class
    public string GenerateClass()
    {
        throw new NotImplementedException();
    }
    
    public string GenerateSqlRequestSelectAll()
    {
        return $"SELECT {GetAllColumns()} FROM {_columnsQuote}{_tableName}{_columnsQuote}";
    }

    public string GenerateSqlRequestSelectById()
    {
        return $"SELECT {GetAllColumns()} FROM {_columnsQuote}{_tableName}{_columnsQuote} {GetWhereId()}";
    }

    public string GenerateSqlRequestInsert()
    {
        return $"INSERT INTO {_columnsQuote}{_tableName}{_columnsQuote} ({GetAllColumns()}) VALUES ({GetAllParameters()})";
    }

    public string GenerateSqlRequestUpdate()
    {
        return $"UPDATE {_columnsQuote}{_tableName}{_columnsQuote} SET {GetColumnsSets()} {GetWhereId()}";
    }

    public string GenerateSqlRequestDelete()
    {
        return $"DELETE FROM {_columnsQuote}{_tableName}{_columnsQuote} {GetWhereId()}";
    }
}