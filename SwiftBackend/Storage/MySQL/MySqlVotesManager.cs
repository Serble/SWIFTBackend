using MySql.Data.MySqlClient;
using SwiftBackend.Schemas;

namespace SwiftBackend.Storage.MySQL; 

public partial class MySqlStorage {

    public async Task<SiteVoteProfile> GetSiteVotes(string domain) {
        SiteVoteProfile profile = new() {
            Domain = domain,
            SafeScore = 0,
            UnsafeScore = 0,
            TotalVotes = 0
        };
        MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(_connectString, @$"SELECT  
   domain,
   SUM(IF(value > 0, value, 0)) AS safeScore,
   SUM(IF(value < 0, value, 0)) AS unsafeScore,
   COUNT(*) AS totalVotes 
FROM  
   {_tablePrefix}votes 
WHERE
   domain = '{domain}'
GROUP BY
   domain;");
        if (!reader.HasRows) {
            await reader.DisposeAsync();
            return profile;
        }
        reader.Read();
        profile.Domain = domain;
        profile.SafeScore = reader.GetInt32("safeScore");
        profile.UnsafeScore = reader.GetInt32("unsafeScore");
        profile.TotalVotes = reader.GetInt32("totalVotes");
        await reader.DisposeAsync();
        return profile;
    }

    public async Task SubmitVote(UserVote vote) {
        await MySqlHelper.ExecuteNonQueryAsync(_connectString, @$"INSERT INTO {_tablePrefix}votes (id, domain, user_id, value) VALUES (@id, @domain, @user_id, @value) ON DUPLICATE KEY UPDATE value=@value", new MySqlParameter[] {
            new("@id", new Guid().ToString()),
            new("@domain", vote.Domain),
            new("@user_id", vote.UserId),
            new("@value", vote.Vote)
        });
    }

    public async Task<int?> GetUserVote(string userId, string domain) {
        MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(_connectString, @$"SELECT * FROM {_tablePrefix}votes WHERE user_id='{userId}' AND domain='{domain}'");
        if (!reader.HasRows) {
            await reader.DisposeAsync();
            return null;
        }
        reader.Read();
        int vote = reader.GetInt32("value");
        await reader.DisposeAsync();
        return vote;
    }
}