using System.Text.Json.Serialization;

namespace SwiftBackend.Schemas; 

public class SwiftUser {
    
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;
    
    [JsonPropertyName("createdAt")]
    public long CreatedAt { get; set; } = 0;
    
    [JsonPropertyName("premium")]
    public bool Premium { get; set; } = false;
    
    [JsonPropertyName("admin")]
    public bool Admin { get; set; } = false;
}