using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class Auth0ManagementService
{
    private readonly HttpClient _httpClient;
    private readonly string _domain;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public Auth0ManagementService(HttpClient httpClient, string domain, string clientId, string clientSecret)
    {
        _httpClient = httpClient;
        _domain = domain;
        _clientId = clientId;
        _clientSecret = clientSecret;
    }
    

    public async Task<string> GetAccessTokenAsync()
    {
        var payload = new
        {
            client_id = _clientId,
            client_secret = _clientSecret,
            audience = $"https://{_domain}/api/v2/",
            grant_type = "client_credentials"
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"https://{_domain}/oauth/token", content);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<JsonElement>(json);

        return tokenResponse.GetProperty("access_token").GetString();
    }

    public async Task<string> GetUserAsync(string userId)
    {
        //var token = await GetAccessTokenAsync();
        //_httpClient.DefaultRequestHeaders.Authorization =
        //    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //var response = await _httpClient.GetAsync($"https://{_domain}/api/v2/users/{userId}");
        //response.EnsureSuccessStatusCode();

        //return await response.Content.ReadAsStringAsync();

        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync($"https://{_domain}/api/v2/users/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            // Log the error or throw an exception with the error content
            throw new Exception($"Error fetching user data: {errorContent}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    public async Task DeleteUserAsync(string userId)
    {
        var token = await GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"https://{_domain}/api/v2/users/{userId}");
        response.EnsureSuccessStatusCode();
    }

    
}