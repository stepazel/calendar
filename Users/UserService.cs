using System.Data;
using Dapper;
using Microsoft.AspNetCore.Components.Authorization;

namespace Calendar.Users;

public class UserService
{
    private readonly IDbConnection _db;
    private readonly AuthenticationStateProvider _authStateProvider;

    public UserService(IDbConnection db, AuthenticationStateProvider authStateProvider)
    {
        _db = db;
        _authStateProvider = authStateProvider;
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

    public async Task<User?> GetUserAsync(int id)
    {
        const string sql = "select * from Users where Id = @id";
        return await _db.QuerySingleOrDefaultAsync<User?>(sql, new { id });
    }

    public int? GetCurrentUserId()
    {
        var authState = _authStateProvider.GetAuthenticationStateAsync().Result;
        var user = authState.User;
        if (user.Identity?.IsAuthenticated is not true) return null;
        
        var userId = user.FindFirst(c => c.Type == "UserId");
        if (userId != null)
            return int.Parse(userId.Value);
        
        return null;
    }
}