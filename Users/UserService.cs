using System.Data;
using Dapper;

namespace Calendar.Users;

public class UserService
{
    private readonly IDbConnection _db;

    public UserService(IDbConnection db)
    {
        _db = db;
    }

    public async Task<int> CreateUserAsync(string name, string email)
    {
        const string sql = "insert into Users (Name, Email) values (@name, @email) select cast(scope_identity() as int)";
        return await _db.QuerySingleAsync<int>(sql, new { name, email });
    }

    public async Task<User?> GetUserAsync(string email)
    {
        const string sql = "select * from Users where Email = @email";
        return await _db.QuerySingleOrDefaultAsync<User?>(sql, new { email });
    }
}