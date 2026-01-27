using Microsoft.Extensions.Logging;
using PetCare.Shared.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace PetCare.MobileApp.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task AddAuthorizationHeaderAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                await AddAuthorizationHeaderAsync();

                _logger.LogInformation("HTTP GET: {Endpoint}", endpoint);

                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default;
                    }

                    return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        _logger.LogWarning("HTTP 401 Unauthorized for {Endpoint}. Token might be expired.", endpoint);
                    }
                    else
                    {
                        _logger.LogError("HTTP GET Error for {Endpoint}. Status: {Status}", endpoint, response.StatusCode);
                    }

                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical connection error in GET {Endpoint}", endpoint);
                return default;
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                await AddAuthorizationHeaderAsync();

                _logger.LogInformation("HTTP POST: {Endpoint}", endpoint);

                var response = await _httpClient.PostAsJsonAsync(endpoint, data);

                if (response.Content != null)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("API Response [{Status}]: {Content}", response.StatusCode, jsonString);
                    }
                    else
                    {
                        _logger.LogDebug("API Success: {Content}", jsonString);
                    }

                    try
                    {
                        var result = JsonSerializer.Deserialize<TResponse>(jsonString, _jsonOptions);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "JSON Deserialization error. Raw content: {Content}", jsonString);
                        throw new Exception($"Error reading response. Status: {response.StatusCode}");
                    }
                }

                throw new Exception($"Empty response. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error in POST {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task PostAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                _logger.LogInformation("HTTP POST (No Return): {Endpoint}", endpoint);

                var response = await _httpClient.PostAsJsonAsync(endpoint, data);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("HTTP POST Failed. Status: {Status}. Content: {Content}", response.StatusCode, errorContent);
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in POST (No Return) {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<List<PetReadModel>> GetPetsAsync(int? ownerId = null)
        {
            var url = "api/pets";

            if (ownerId.HasValue)
            {
                url += $"?ownerId={ownerId.Value}";
            }
            var result = await GetAsync<List<PetReadModel>>(url);

            return result ?? new List<PetReadModel>();
        }
    }
}
