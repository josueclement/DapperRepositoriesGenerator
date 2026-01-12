using System;
using System.Collections.Generic;
using System.Data;

namespace DapperRepositoriesGenerator;

public class DbTableGenerator(Func<IDbConnection> connectionFactory)
{
    public IEnumerable<DbTable> Generate(params string[] tableNames)
    {
        var tables = new List<DbTable>();
        
        using var connection = connectionFactory();
        connection.Open();

        foreach (var tableName in tableNames)
        {
            List<(string columnName, string typeName)> columns = [];
            using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM `{tableName}` WHERE 1=0";
            using var reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
        
            var schemaTable = reader.GetSchemaTable() ?? throw new InvalidOperationException($"No schema returned for table {tableName}.");
            foreach (DataRow row in schemaTable.Rows)
            {
                 var columnName = row["ColumnName"].ToString();
                 var dataType = (Type)row["DataType"];
                 var strType = GetCSharpTypeName(dataType);
            
                columns.Add((columnName, strType));
            }
            
            tables.Add(new DbTable(tableName, columns.ToArray()));
        }
        
        connection.Close();
        
        return tables;
    }
    
    public static string GetCSharpTypeName(Type type)
    {
        var typeMap = new Dictionary<Type, string>
        {
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(char), "char" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(float), "float" },
            { typeof(int), "int" },
            { typeof(uint), "uint" },
            { typeof(long), "long" },
            { typeof(ulong), "ulong" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(string), "string" }
        };

        // Return mapped name or the type's name if not in map
        return typeMap.TryGetValue(type, out var alias) ? alias : type.Name;
    }
}