using PetCare.MobileApp.Common;
using PetCare.MobileApp.Models.Appointments;
using PetCare.MobileApp.Models.Vets;
using PetCare.Shared.Dtos;

namespace PetCare.MobileApp.Services
{
    public interface IApiService
    {
        Task<T?> GetAsync<T>(string endpoint);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        //Post bez zwracania wyniku
        Task PostAsync<TRequest>(string endpoint, TRequest data);
        Task PutAsync<TRequest>(string endpoint, TRequest data);
        Task DeleteAsync(string endpoint);
        Task<List<PetReadModel>> GetPetsAsync(int? ownerId = null);
        Task<PetDetailDto?> GetPetDetailsAsync(int petId);
        Task AddPetAsync(PetCreateModel petModel);
        Task UpdatePetAsync(int petId, PetUpdateModel petModel);
        Task DeletePetAsync(int petId);
        Task<PaginatedResult<AppointmentReadModel>> GetMyAppointmentsAsync
            (int ownerId, bool upcomingOnly, int page = 1, int pageSize = 10);
        Task<List<VetLookupDto>> GetVetsForLookupAsync();
        Task<List<TimeSpan>> GetVetAvailabilityAsync(int vetId, DateTime date);
        Task CreateAppointmentAsync(AppointmentCreateModel model);
    }
}
