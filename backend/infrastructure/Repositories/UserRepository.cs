﻿using Dapper;
using infrastructure.Models;
using Npgsql;

namespace infrastructure.Repositories;

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource), "DataSource is null");
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        const string sql = "SELECT * FROM users WHERE username = @Username;";
        using (var conn = _dataSource.OpenConnection())
        {
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
        }
    }

    public async Task CreateUserAsync(User user)
    {
        const string sql = "INSERT INTO users (username, email, passwordhash, passwordsalt) VALUES (@Username, @Email, @PasswordHash, @PasswordSalt);";
        using (var conn = _dataSource.OpenConnection())
        {
            await conn.ExecuteAsync(sql, user);
        }
    }
}