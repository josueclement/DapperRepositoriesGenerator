using System.Collections.Generic;
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

// using System;
// using System.Data;
// using System.Data.SqlClient;
//
// string connectionString = "your_connection_string";
// string query = "SELECT * FROM YourTable WHERE 1=0"; // No rows needed, just schema
//
// using (SqlConnection connection = new SqlConnection(connectionString))
// {
//     connection.Open();
//     using (SqlCommand command = new SqlCommand(query, connection))
//     {
//         using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
//         {
//             DataTable schemaTable = reader.GetSchemaTable();
//             
//             foreach (DataRow row in schemaTable.Rows)
//             {
//                 string columnName = row["ColumnName"].ToString();
//                 Type dataType = (Type)row["DataType"]; // C# type
//                 string dbType = row["DataTypeName"].ToString(); // SQL type
//                 
//                 Console.WriteLine($"{columnName}: {dataType.Name} (SQL: {dbType})");
//             }
//         }
//     }
// }