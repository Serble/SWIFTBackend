using System.Text.Json.Serialization;

namespace SwiftBackend.Schemas; 

public class SiteVoteProfile {
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = null!;
    
    [JsonPropertyName("safeScore")]
    public int SafeScore { get; set; }
    
    [JsonPropertyName("unsafeScore")]
    public int UnsafeScore { get; set; }
    
    [JsonPropertyName("totalVotes")]
    public int TotalVotes { get; set; }
}