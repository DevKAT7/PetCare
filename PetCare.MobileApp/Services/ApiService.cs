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

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("HTTP GET Error {Status}: {Content}", response.StatusCode, error);
                    throw new HttpRequestException($"Request failed: {response.StatusCode}. {error}");
                }

                return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Critical error in GET {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                _logger.LogInformation("HTTP POST: {Endpoint}", endpoint);

                var response = await _httpClient.PostAsJsonAsync(endpoint, data);

                var jsonString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API Error {Status}: {Content}", response.StatusCode, jsonString);
                    throw new HttpRequestException($"Request failed: {response.StatusCode}. {jsonString}");
                }

                try
                {
                    _logger.LogDebug("API Success: {Content}", jsonString);

                    return JsonSerializer.Deserialize<TResponse>(jsonString, _jsonOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "JSON Deserialization error. Raw content: {Content}", jsonString);
                    throw new Exception("Error parsing API response", ex);
                }
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
                    throw new HttpRequestException($"POST failed: {response.StatusCode}. {errorContent}");       
                }

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in POST (No Return) {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task PutAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                _logger.LogInformation("HTTP PUT: {Endpoint}", endpoint);

                var response = await _httpClient.PutAsJsonAsync(endpoint, data);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"PUT failed: {response.StatusCode}. {error}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PUT {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task DeleteAsync(string endpoint)
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                _logger.LogInformation("HTTP DELETE: {Endpoint}", endpoint);

                var response = await _httpClient.DeleteAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"DELETE failed: {response.StatusCode}. {error}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DELETE {Endpoint}", endpoint);
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

        public async Task<PetDetailDto?> GetPetDetailsAsync(int petId)
        {
            return await GetAsync<PetDetailDto>($"api/pets/{petId}");
        }

        public async Task AddPetAsync(PetCreateModel petModel)
        {
            await PostAsync("api/pets", petModel);
        }

        public async Task UpdatePetAsync(int petId, PetUpdateModel petModel)
        {
            await PutAsync($"api/pets/{petId}", petModel);
        }

        public async Task DeletePetAsync(int petId)
        {
            await DeleteAsync($"api/pets/{petId}");
        }
    }
}
