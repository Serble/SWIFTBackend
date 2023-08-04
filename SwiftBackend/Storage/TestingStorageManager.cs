using GeneralPurposeLib;
using SwiftBackend.Schemas;

namespace SwiftBackend.Storage; 

public class TestingStorageManager : IStorageManager {
    public readonly List<SwiftUser> Users = new();
    public readonly List<UserVote> Votes = new();

    public void Init() {
        Logger.Warn("Using testing storage manager");
    }

    public void Deinit() { }

    public Task<SwiftUser?> GetUser(string id) {
        return Task.FromResult(Users.FirstOrDefault(user => user.Id == id));
    }

    public Task CreateUser(SwiftUser user) {
        Users.Add(user);
        return Task.CompletedTask;
    }

    public Task EditUser(SwiftUser user) {
        Users.First(u => u.Id == user.Id).Username = user.Username;
        return Task.CompletedTask;
    }

    public Task<SiteVoteProfile> GetSiteVotes(string domain) {
        SiteVoteProfile votes = new();
        UserVote[] votesForDomain = Votes.Where(vote => vote.Domain == domain).ToArray();
        votes.Domain = domain;
        votes.SafeScore = votesForDomain.Where(vote => vote.Vote > 0).Sum(vote => vote.Vote);
        votes.UnsafeScore = votesForDomain.Where(vote => vote.Vote < 0).Sum(vote => vote.Vote);
        votes.TotalVotes = votesForDomain.Length;
        return Task.FromResult(votes);
    }

    public Task SubmitVote(UserVote vote) {
        Votes.RemoveAll(v => v.Domain == vote.Domain && v.UserId == vote.UserId);
        Votes.Add(vote);
        return Task.CompletedTask;
    }

    public Task<int?> GetUserVote(string userId, string domain) {
        UserVote? vote = Votes.FirstOrDefault(vote => vote.UserId == userId && vote.Domain == domain);
        return vote == null ? Task.FromResult<int?>(null) : Task.FromResult<int?>(vote.Vote);
    }
}