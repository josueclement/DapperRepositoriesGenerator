using System.Linq;

namespace DapperRepositoriesGenerator;

public static class DbTableExtensions
{
    extension(DbTable table)
    {
        public string GetIdColumnName()
            => table.ColumnNames.First();
        
        public string GetIdTypeName()
            => table.Columns.First().typeName;

        public string GetIdParameterName()
        {
            var id = table.GetIdColumnName();
            return id.Substring(0, 1).ToLower() + id.Substring(1);
        }
        
        public string GetTableParameterName()
            => table.TableName.Substring(0, 1).ToLower() + table.TableName.Substring(1);
    }
}