using System.Net.Http.Json;

namespace PetCare.MobileApp.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            // Tutaj można dodać obsługę błędów (try-catch)
            // lub logikę dodawania Bearer Tokena (autoryzacja)
            return await _httpClient.GetFromJsonAsync<T>(endpoint);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            // Rzuć wyjątek, jeśli API zwróci błąd (np. 400 lub 500)
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PostAsync<TRequest>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
        }
    }
}
