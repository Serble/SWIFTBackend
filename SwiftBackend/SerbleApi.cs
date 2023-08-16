using GeneralPurposeLib;
using SwiftBackend.Schemas;

namespace SwiftBackend; 

public class SerbleApi {
    private const string APIURL = "https://api.serble.net/api/v1";
    private string _accessToken;

    public SerbleApi(string accessToken = null) {
        _accessToken = accessToken;
    }
    
    private void EnsureAccessToken() {
        if (_accessToken == null) {
            throw new InvalidOperationException("No access token");
        }
    }

    private HttpClient GetHttpClient() {
        HttpClient client = new();
        if (_accessToken == null) {
            return client;
        }
        client.DefaultRequestHeaders.Add("SerbleAuth", "App " + _accessToken);
        return client;
    }

    public async Task<AuthCodeExchangeResponse> ExchangeAuthCode(string authcode) {
        HttpClient client = GetHttpClient();
        try {
            string url = APIURL +
                         "/oauth/token/refresh?" +
                         $"code={authcode}&" +
                         $"client_id={GlobalConfig.Config["serble_app_id"]}&" +
                         $"client_secret={GlobalConfig.Config["serble_app_secret"]}&" +
                         "grant_type=authorization_code";
            HttpResponseMessage response = await client.PostAsync(url, null);
            AuthCodeExchangeResponse serbleResponse = await response.Content.ReadFromJsonAsync<AuthCodeExchangeResponse>() ?? throw new InvalidOperationException();
            _accessToken = serbleResponse.AccessToken;
            return serbleResponse;
        }
        catch (Exception) {
            return null;  // Failed
        }
    }
    
    public async Task<AuthCodeExchangeResponse> Refresh(string refreshToken) {
        HttpClient client = GetHttpClient();
        try {
            string url = APIURL +
                         "/oauth/token/access?" +
                         $"refresh_token={refreshToken}&" +
                         $"client_id={GlobalConfig.Config["serble_app_id"]}&" +
                         $"client_secret={GlobalConfig.Config["serble_app_secret"]}&" +
                         "grant_type=authorization_code";
            HttpResponseMessage response = (await client.PostAsync(url, null)).EnsureSuccessStatusCode();
            AuthCodeExchangeResponse serbleResponse = await response.Content.ReadFromJsonAsync<AuthCodeExchangeResponse>() ?? throw new InvalidOperationException();
            _accessToken = serbleResponse.AccessToken;
            return serbleResponse;
        }
        catch (Exception) {
            return null;  // Failed
        }
    }

    public async Task<SerbleUser> GetUser() {
        EnsureAccessToken();
        HttpClient client = GetHttpClient();
        try {
            const string url = APIURL + "/account";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<SerbleUser>() ?? throw new InvalidOperationException();
        }
        catch (Exception) {
            return null;  // Failed
        }
    }
    
    public async Task<string[]> GetOwnedProducts() {
        EnsureAccessToken();
        HttpClient client = GetHttpClient();
        try {
            const string url = APIURL + "/products";
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<string[]>() ?? throw new InvalidOperationException();
        }
        catch (Exception) {
            return null;  // Failed
        }
    }
    
}