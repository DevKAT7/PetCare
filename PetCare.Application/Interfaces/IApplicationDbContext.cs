using Microsoft.EntityFrameworkCore;
using PetCare.Core.Models;

namespace PetCare.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Vet> Vets { get; set; }
        DbSet<VetSpecialization> VetSpecializations { get; set; }
        DbSet<VetSpecializationLink> VetSpecializationLinks { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<VetSchedule> VetSchedules { get; set; }
        DbSet<ScheduleException> ScheduleExceptions { get; set; }
        DbSet<PetOwner> PetOwners { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<InvoiceItem> InvoiceItems { get; set; }
        DbSet<Pet> Pets { get; set; }
        DbSet<MedicalTest> MedicalTests { get; set; }
        DbSet<Vaccination> Vaccinations { get; set; }
        DbSet<Appointment> Appointments { get; set; }
        DbSet<AppointmentProcedure> AppointmentProcedures { get; set; }
        DbSet<Procedure> Procedures { get; set; }
        DbSet<Prescription> Prescriptions { get; set; }
        DbSet<Medication> Medications { get; set; }
        DbSet<StockItem> StockItems { get; set; }
        DbSet<StockTransaction> StockTransactions { get; set; }
        DbSet<PageText> PageTexts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
