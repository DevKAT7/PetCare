using PetCare.MobileApp.Common;
using PetCare.MobileApp.Models.Appointments;
using PetCare.MobileApp.Models.Vets;
using PetCare.MobileApp.Models.Pets;
using PetCare.MobileApp.Models.Invoices;

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
        Task<AppointmentReadModel?> GetAppointmentDetailsAsync(int appointmentId);
        Task ConfirmAppointmentAsync(int appointmentId);
        Task CancelAppointmentAsync(int appointmentId);
        Task<List<InvoiceReadModel>> GetMyInvoicesAsync(int ownerId);
        Task<InvoiceReadModel?> GetInvoiceAsync(int invoiceId);
        Task MarkInvoicePaidAsync(int invoiceId, DateTime paymentDate);
        Task<byte[]> GetInvoicePdfAsync(int invoiceId);
        Task<byte[]> GetPrescriptionPdfAsync(int prescriptionId);
        Task<byte[]> GetMedicalTestAttachmentAsync(int testId);
    }
}
