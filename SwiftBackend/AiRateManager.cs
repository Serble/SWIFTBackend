using System.Text;
using System.Text.Json;
using GeneralPurposeLib;

namespace SwiftBackend; 

public static class AiRateManager {
    
    public static async Task<int> GetBotRating(string domain) {
        HttpClient client = new();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {GlobalConfig.Config["open_ai_token"]}");
        client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        dynamic requestBody = new {
            model = "gpt-3.5-turbo",
            messages = new[] {
                new {
                    role = "system",
                    content = "Rate the domains out of 100 based on how trustworthy they might be. Only send the rating, nothing else."
                },
                new {
                    role = "user",
                    content = domain
                }
            },
        };
        HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/engines/davinci/completions", new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8));
        if (!response.IsSuccessStatusCode) {
            Logger.Error("Failed to get bot rating");
            Logger.Error("Response: " + await response.Content.ReadAsStringAsync());
            return 75;
        }
        dynamic responseBody = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        string rating = responseBody.choices[0].message.content;
        if (!int.TryParse(rating, out int ratingInt)) {
            Logger.Error("Failed to get bot rating");
            Logger.Error("Response: " + await response.Content.ReadAsStringAsync());
            return 75;
        }
        await Program.StorageManager.SetBotRating(domain, ratingInt);
        return ratingInt;
    }
    
}