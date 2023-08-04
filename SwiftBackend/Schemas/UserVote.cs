namespace SwiftBackend.Schemas; 

public class UserVote {
    public string UserId { get; set; } = null!;
    public string Domain { get; set; } = null!;
    public int Vote { get; set; }
}