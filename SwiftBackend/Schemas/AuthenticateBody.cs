using System.Text.Json.Serialization;

namespace SwiftBackend.Schemas; 

public class AuthenticateBody {
    [JsonPropertyName("token")] 
    public string? Token { get; set; } = null;
}