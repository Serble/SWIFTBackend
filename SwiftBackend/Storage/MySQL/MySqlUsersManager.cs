using MySql.Data.MySqlClient;
using SwiftBackend.Schemas;

namespace SwiftBackend.Storage.MySQL; 

public partial class MySqlStorage {
    
    public async Task<SwiftUser?> GetUser(string id) {
        MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(_connectString, @$"SELECT * FROM {_tablePrefix}users WHERE id='{id}'");
        if (!reader.HasRows) {
            await reader.DisposeAsync();
            return null;
        }
        reader.Read();
        SwiftUser user = new() {
            Id = reader.GetString("id"),
            Username = reader.GetString("username"),
            CreatedAt = reader.GetDateTime("created_at").DateTimeToUnixMillis(),
            Premium = reader.GetBoolean("premium"),
            Admin = reader.GetBoolean("admin")
        };
        await reader.DisposeAsync();
        return user;
    }
    
    public async Task CreateUser(SwiftUser user) {
        await MySqlHelper.ExecuteNonQueryAsync(_connectString, @$"INSERT INTO {_tablePrefix}users (id, username, created_at, premium, admin) VALUES (@id, @username, @created_at, @premium, @admin)", new MySqlParameter[] {
            new("@id", user.Id),
            new("@username", user.Username),
            new("@created_at", user.CreatedAt.UnixMillisToDateTime()),
            new("@premium", user.Premium),
            new("@admin", user.Admin)
        });
    }
    
    public async Task EditUser(SwiftUser user) {
        await MySqlHelper.ExecuteNonQueryAsync(_connectString, @$"UPDATE {_tablePrefix}users SET username=@username, premium=@premium WHERE id=@id", new MySqlParameter[] {
            new("@id", user.Id),
            new("@username", user.Username)
        });
    }
    
}