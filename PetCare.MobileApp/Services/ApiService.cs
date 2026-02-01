using Microsoft.Extensions.Logging;
using PetCare.MobileApp.Common;
using PetCare.MobileApp.Enums;
using PetCare.MobileApp.Models.Appointments;
using PetCare.MobileApp.Models.Invoices;
using PetCare.MobileApp.Models.Pets;
using PetCare.MobileApp.Models.Vets;
using PetCare.Shared.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
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

                try
                {
                    var result = JsonSerializer.Deserialize<TResponse>(jsonString, _jsonOptions);

                    if (!response.IsSuccessStatusCode && result != null)
                    {
                        _logger.LogWarning("API returned logical error {Status}: {Content}", response.StatusCode, jsonString);
                        return result;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogDebug("API Success: {Content}", jsonString);
                        return result;
                    }
                }
                catch (Exception)
                {
                }

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("API Error {Status}: {Content}", response.StatusCode, jsonString);
                    throw new HttpRequestException($"Request failed: {response.StatusCode}. {jsonString}");
                }

                return default;
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

        public async Task<UserProfileDto?> GetProfileAsync()
        {
            return await GetAsync<UserProfileDto>("api/account/profile");
        }

        public async Task<bool> UpdateProfileAsync(EditProfileDto model)
        {
            try
            {
                await PutAsync("api/account/profile", model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update profile");
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto model)
        {
            try
            {
                await PostAsync("api/account/change-password", model);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to change password");
                return false;
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

        public async Task<PaginatedResult<AppointmentReadModel>> GetMyAppointmentsAsync
            (int ownerId, bool upcomingOnly, int page = 1, int pageSize = 10, int? petId = null,
            DateTime? from = null, DateTime? to = null, AppointmentStatus? status = null)
        {
            var url = $"api/appointments?petOwnerId={ownerId}&pageIndex={page}&pageSize={pageSize}";

            if (from.HasValue)
            {
                url += $"&from={from.Value:yyyy-MM-dd}";
            }
            else if (upcomingOnly)
            {
                url += $"&from={DateTime.Today:yyyy-MM-dd}&sortColumn=Date&sortDirection=asc";
            }

            if (to.HasValue)
            {
                url += $"&to={to.Value:yyyy-MM-dd}";
            }
            else if (!upcomingOnly)
            {
                url += $"&to={DateTime.Today.AddDays(-1):yyyy-MM-dd}&sortColumn=Date&sortDirection=desc";
            }

            if (status.HasValue)
            {
                url += $"&status={status.Value}";
            }

            var result = await GetAsync<PaginatedResult<AppointmentReadModel>>(url);

            if (result == null)
            {
                return new PaginatedResult<AppointmentReadModel>();
            }

            if (petId.HasValue && result.Items != null)
            {
                var filtered = result.Items.Where(x => x.PetId == petId.Value).ToList();
                return new PaginatedResult<AppointmentReadModel>(filtered, filtered.Count, page, pageSize);
            }

            return result;
        }

        public async Task<List<VetLookupDto>> GetVetsForLookupAsync()
        {
            var result = await GetAsync<List<VetLookupDto>>("api/vets/lookup");
            return result ?? new List<VetLookupDto>();
        }

        public async Task<List<TimeSpan>> GetVetAvailabilityAsync(int vetId, DateTime date)
        {
            var url = $"api/appointments/availability?vetId={vetId}&date={date:yyyy-MM-dd}";

            var result = await GetAsync<List<TimeSpan>>(url);
            return result ?? new List<TimeSpan>();
        }

        public async Task CreateAppointmentAsync(AppointmentCreateModel model)
        {
            await PostAsync("api/appointments", model);
        }

        public async Task<AppointmentReadModel?> GetAppointmentDetailsAsync(int appointmentId)
        {
            return await GetAsync<AppointmentReadModel>($"api/appointments/{appointmentId}");
        }

        public async Task ConfirmAppointmentAsync(int appointmentId)
        {
            await PutAsync<object>($"api/appointments/{appointmentId}/confirm", new { });
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            await DeleteAsync($"api/appointments/{appointmentId}");
        }

        public async Task<List<InvoiceReadModel>> GetMyInvoicesAsync(int ownerId,
            InvoiceStatusFilter? status = null)
        {
            var url = $"api/invoices/by-owner/{ownerId}";

            if (status.HasValue)
            {
                url += $"?status={status.Value.ToString().ToLower()}";
            }

            var result = await GetAsync<List<InvoiceReadModel>>(url);
            return result ?? new List<InvoiceReadModel>();
        }

        public async Task<InvoiceReadModel?> GetInvoiceAsync(int invoiceId)
        {
            return await GetAsync<InvoiceReadModel>($"api/invoices/{invoiceId}");
        }

        public async Task MarkInvoicePaidAsync(int invoiceId, DateTime paymentDate)
        {
            await PostAsync($"api/invoices/{invoiceId}/pay", paymentDate);
        }

        public async Task<byte[]> GetInvoicePdfAsync(int invoiceId)
        {
            await AddAuthorizationHeaderAsync();
            return await _httpClient.GetByteArrayAsync($"api/invoices/{invoiceId}/pdf");
        }

        public async Task<byte[]> GetPrescriptionPdfAsync(int prescriptionId)
        {
            await AddAuthorizationHeaderAsync();
            return await _httpClient.GetByteArrayAsync($"api/prescriptions/{prescriptionId}/pdf");
        }

        public async Task<byte[]> GetMedicalTestAttachmentAsync(int testId)
        {
            await AddAuthorizationHeaderAsync();
            return await _httpClient.GetByteArrayAsync($"api/medicaltests/{testId}/download");
        }
    }
}
