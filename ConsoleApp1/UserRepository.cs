using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace ConsoleApp1;

public class User
{
    public string? Id { get; set; }
    public string? CreationDate { get; set; }
    public string? ModificationDate { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Password { get; set; }
}

public class UserRepository(IDbConnection connection)
{
    public IDbConnection Connection => connection;
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var sql = "SELECT `Id`, `CreationDate`, `ModificationDate`, `Username`, `FullName`, `Password` FROM `User`";
        return await Connection
            .QueryAsync<User>(sql)
            .ConfigureAwait(false);
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var sql =
            "SELECT `Id`, `CreationDate`, `ModificationDate`, `Username`, `FullName`, `Password` FROM `User` WHERE `Id` = @Id";
        return await Connection
            .QuerySingleOrDefaultAsync<User>(sql, new { Id = id })
            .ConfigureAwait(false);
    }

    public async Task<int> AddAsync(User entity)
    {
        var sql =
            "INSERT INTO `User` (`Id`, `CreationDate`, `ModificationDate`, `Username`, `FullName`, `Password`) VALUES (@Id, @CreationDate, @ModificationDate, @Username, @FullName, @Password)";
        return await Connection
            .ExecuteAsync(sql, entity)
            .ConfigureAwait(false);
    }

    public async Task<int> UpdateAsync(User entity)
    {
        var sql =
            "UPDATE `User` SET `CreationDate` = @CreationDate, `ModificationDate` = @ModificationDate, `Username` = @Username, `FullName` = @FullName, `Password` = @Password WHERE `Id` = @Id";
        return await Connection
            .ExecuteAsync(sql, entity)
            .ConfigureAwait(false);
    }

    public async Task<int> DeleteAsync(User entity)
    {
        var sql = "DELETE FROM `User` WHERE `Id` = @Id";
        return await Connection
            .ExecuteAsync(sql, entity)
            .ConfigureAwait(false);
    }
}
