using System;
using System.Text;

namespace DapperRepositoriesGenerator;

public class RepositoryGenerator(DbTable table)
{
    public string GenerateRepository()
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("using Dapper;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Data;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine();
        sb.AppendLine("namespace MyNamespace;");
        sb.AppendLine();
        sb.AppendLine("public class MyRepository");
        sb.AppendLine("{");
        GenerateGetAll(sb);
        sb.AppendLine();
        GenerateGetById(sb);
        sb.AppendLine();
        GenerateInsert(sb);
        sb.AppendLine();
        GenerateUpdate(sb);
        sb.AppendLine();
        GenerateDelete(sb);
        sb.AppendLine("}");
        
        return sb.ToString();
    }
    
    private void GenerateGetAll(StringBuilder sb)
    {
        sb.AppendLine($"    public async Task<IEnumerable<{table.TableName}>> GetAllAsync()");
        sb.AppendLine("    {");
        sb.AppendLine($@"        var sql = ""{table.GenerateSqlRequestSelectAll()}""");
        
        sb.AppendLine("        return await Connection");
        sb.AppendLine($"            .QueryAsync<{table.TableName}>(sql)");
        sb.AppendLine("            .ConfigureAwait(false);");
        sb.AppendLine("    }");
    }
    
    private void GenerateGetById(StringBuilder sb)
    {
        sb.AppendLine($"    public async Task<{table.TableName}?> GetByIdAsync(string id)");
        sb.AppendLine("    {");
        sb.AppendLine($@"        var sql = ""{table.GenerateSqlRequestSelectById()}""");
        
        sb.AppendLine("        return await Connection");
        sb.AppendLine($"            .QuerySingleOrDefaultAsync<{table.TableName}>(sql, new {{ Id = id }})");
        sb.AppendLine("            .ConfigureAwait(false);");
        sb.AppendLine("    }");
    }

    private void GenerateInsert(StringBuilder sb)
    {
        sb.AppendLine($"    public async Task<int> AddAsync({table.TableName} entity)");
        sb.AppendLine("    {");
        sb.AppendLine($@"        var sql = ""{table.GenerateSqlRequestInsert()}""");
        
        sb.AppendLine("        return await Connection");
        sb.AppendLine("            .ExecuteAsync(sql, entity)");
        sb.AppendLine("            .ConfigureAwait(false);");
        sb.AppendLine("    }");
    }

    private void GenerateUpdate(StringBuilder sb)
    {
        sb.AppendLine($"    public async Task<int> UpdateAsync({table.TableName} entity)");
        sb.AppendLine("    {");
        sb.AppendLine($@"        var sql = ""{table.GenerateSqlRequestUpdate()}""");
        
        sb.AppendLine("        return await Connection");
        sb.AppendLine("            .ExecuteAsync(sql, entity)");
        sb.AppendLine("            .ConfigureAwait(false);");
        sb.AppendLine("    }");
    }

    private void GenerateDelete(StringBuilder sb)
    {
        sb.AppendLine($"    public async Task<int> DeleteAsync({table.TableName} entity)");
        sb.AppendLine("    {");
        sb.AppendLine($@"        var sql = ""{table.GenerateSqlRequestDelete()}""");
        
        sb.AppendLine("        return await Connection");
        sb.AppendLine("            .ExecuteAsync(sql, entity)");
        sb.AppendLine("            .ConfigureAwait(false);");
        sb.AppendLine("    }");
    }
}