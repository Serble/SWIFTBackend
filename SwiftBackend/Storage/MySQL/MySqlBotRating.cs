using MySql.Data.MySqlClient;

namespace SwiftBackend.Storage.MySQL; 

public partial class MySqlStorage {
    public async Task<int> GetBotRating(string domain) {
        MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(_connectString, @$"SELECT * FROM {_tablePrefix}bot_rating WHERE domain='{domain}'");
        if (!reader.HasRows) {
            await reader.DisposeAsync();
            return -1;
        }
        reader.Read();
        int rating = reader.GetInt32("rating");
        await reader.DisposeAsync();
        return rating;
    }

    public async Task SetBotRating(string domain, int rating) {
        await MySqlHelper.ExecuteNonQueryAsync(_connectString, @$"INSERT INTO {_tablePrefix}bot_rating (id, domain, rating) VALUES (@id, @domain, @rating) ON DUPLICATE KEY UPDATE rating=@rating", new MySqlParameter[] {
            new("@id", Guid.NewGuid().ToString()),
            new("@domain", domain),
            new("@rating", rating)
        });
    }
}