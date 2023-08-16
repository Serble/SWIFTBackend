using SwiftBackend.Schemas;

namespace SwiftBackend.Storage; 

public interface IStorageManager {
    void Init();
    void Deinit();

    Task<SwiftUser> GetUser(string id);
    Task CreateUser(SwiftUser user);
    Task EditUser(SwiftUser user);
    
    Task<SiteVoteProfile> GetSiteVotes(string domain);
    Task SubmitVote(UserVote vote);
    Task<int?> GetUserVote(string userId, string domain);
    
    Task<int> GetBotRating(string id);
    Task SetBotRating(string id, int rating);
}