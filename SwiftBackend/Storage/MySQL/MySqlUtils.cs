using MySql.Data.MySqlClient;
using SwiftBackend.Schemas;

namespace SwiftBackend.Storage.MySQL;

public partial class MySqlStorage {

    private static SwiftUser CompileChatUser(MySqlDataReader reader) {
        SwiftUser user = new() {
            Id = reader.GetString("id"),
            Username = reader.GetString("username"),
            CreatedAt = reader.GetDateTime("created_at").DateTimeToUnixMillis()
        };
        return user;
    }
    
}