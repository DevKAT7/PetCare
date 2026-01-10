using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetCare.Application.Interfaces;
using PetCare.Core.Models;

namespace PetCare.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vet> Vets { get; set; }
        public DbSet<VetSpecialization> VetSpecializations { get; set; }
        public DbSet<VetSpecializationLink> VetSpecializationLinks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<VetSchedule> VetSchedules { get; set; }
        public DbSet<ScheduleException> ScheduleExceptions { get; set; }
        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<MedicalTest> MedicalTests { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentProcedure> AppointmentProcedures { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vet>(e =>
            {
                e.HasIndex(vet => vet.Pesel).IsUnique();
                e.HasIndex(vet => vet.LicenseNumber).IsUnique();
                e.HasOne(vet => vet.User)
                    .WithOne(u => u.VetProfile)
                    .HasForeignKey<Vet>(vet => vet.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<VetSpecialization>(e =>
            {
                e.HasMany(vs => vs.Procedures)
                    .WithOne(p => p.VetSpecialization)
                    .HasForeignKey(p => p.VetSpecializationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<VetSpecializationLink>(e =>
            {
                e.HasKey(link => new { link.VetId, link.VetSpecializationId });
                e.HasOne(link => link.Vet)
                    .WithMany(vet => vet.SpecializationLinks)
                    .HasForeignKey(link => link.VetId);
                e.HasOne(link => link.VetSpecialization)
                    .WithMany(spec => spec.VetLinks)
                    .HasForeignKey(link => link.VetSpecializationId);
            });

            builder.Entity<VetSchedule>(e =>
            {
                e.Property(e => e.DayOfWeek)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                e.HasOne(vs => vs.Vet)
                    .WithMany()
                    .HasForeignKey(vs => vs.VetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ScheduleException>(e =>
            {
                e.HasOne(se => se.Vet)
                    .WithMany()
                    .HasForeignKey(se => se.VetId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(se => new { se.VetId, se.ExceptionDate }).IsUnique();
            });

            builder.Entity<AppointmentProcedure>(e =>
            {
                e.HasKey(ap => new { ap.AppointmentId, ap.ProcedureId });
                e.HasOne(ap => ap.Appointment)
                    .WithMany(a => a.AppointmentProcedures)
                    .HasForeignKey(ap => ap.AppointmentId);
                e.HasOne(ap => ap.Procedure)
                    .WithMany(p => p.AppointmentProcedures)
                    .HasForeignKey(ap => ap.ProcedureId);
            });

            builder.Entity<Appointment>(e =>
            {
                e.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                e.HasOne(a => a.Pet)
                    .WithMany()
                    .HasForeignKey(a => a.PetId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(a => a.Vet)
                    .WithMany()
                    .HasForeignKey(a => a.VetId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany<Vaccination>()
                    .WithOne(v => v.Appointment)
                    .HasForeignKey(v => v.AppointmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany<MedicalTest>()
                    .WithOne(t => t.Appointment)
                    .HasForeignKey(t => t.AppointmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany<Prescription>()
                    .WithOne(p => p.Appointment)
                    .HasForeignKey(p => p.AppointmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Notification>(e =>
            {
                e.HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<PetOwner>(e =>
            {
                e.HasOne(o => o.User)
                    .WithOne(u => u.PetOwnerProfile)
                    .HasForeignKey<PetOwner>(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(o => o.Pets)
                    .WithOne(p => p.PetOwner)
                    .HasForeignKey(p => p.PetOwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Invoice>(e =>
            {
                e.HasIndex(i => i.InvoiceNumber).IsUnique();

                e.HasMany(i => i.InvoiceItems)
                    .WithOne(item => item.Invoice)
                    .HasForeignKey(item => item.InvoiceId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(i => i.Appointment)
                    .WithOne()
                    .HasForeignKey<Invoice>(i => i.AppointmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(i => i.PetOwner)
                    .WithMany(po => po.Invoices)
                    .HasForeignKey(i => i.PetOwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Pet>(e =>
            {
                e.HasMany(p => p.Vaccinations)
                    .WithOne(v => v.Pet)
                    .HasForeignKey(v => v.PetId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(p => p.MedicalTests)
                    .WithOne(t => t.Pet)
                    .HasForeignKey(t => t.PetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            builder.Entity<Medication>(e =>
            {
                e.HasIndex(m => m.Name).IsUnique();

                e.HasMany(m => m.Prescriptions)
                    .WithOne(p => p.Medication)
                    .HasForeignKey(p => p.MedicationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<StockItem>(e =>
            {
                e.HasOne(si => si.Medication)
                    .WithOne(m => m.StockItem)
                    .HasForeignKey<StockItem>(si => si.MedicationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<StockTransaction>(e =>
            {
                e.HasOne(st => st.Medication)
                    .WithMany(m => m.StockTransactions)
                    .HasForeignKey(st => st.MedicationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
