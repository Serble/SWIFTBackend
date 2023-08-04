using System.Text.Json.Serialization;

namespace SwiftBackend.Schemas; 

public class SerbleUser {
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("verifiedEmail")]
    public bool VerifiedEmail { get; set; }
    
    [JsonPropertyName("permLevel")]
    public int PermLevel{ get; set; }
    
    [JsonPropertyName("premiumLevel")]
    public int PremiumLevel{ get; set; }
    
    [JsonPropertyName("permString")]
    public string? PermString{ get; set; }
    
    [JsonPropertyName("authorizedApps")]
    public object[]? AuthorizedApps{ get; set; }
    
    [JsonPropertyName("language")]
    public string? Language{ get; set; }
}