using System.Text.Json.Serialization;

namespace SwiftBackend.Schemas; 

public class AuthCodeExchangeResponse {
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = null!;

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = null!;
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = null!;
    
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }
}