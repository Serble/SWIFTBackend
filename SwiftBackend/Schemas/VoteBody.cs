using System.Text.Json.Serialization;

namespace SwiftBackend.Schemas; 

public class VoteBody {
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = null!;
    
    [JsonPropertyName("vote")]
    public bool Vote { get; set; }
}