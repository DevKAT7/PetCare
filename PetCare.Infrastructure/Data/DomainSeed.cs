using Microsoft.EntityFrameworkCore;
using PetCare.Core.Models;

namespace PetCare.Infrastructure.Data
{
    public static class DomainSeed
    {
        public static async Task SeedSpecializationsAsync(ApplicationDbContext context)
        {
            if (await context.VetSpecializations.AnyAsync())
            {
                return;
            }

            var specializations = new List<VetSpecialization>
            {
                new VetSpecialization { Name = "General Practitioner" },
                new VetSpecialization { Name = "Internist" },
                new VetSpecialization { Name = "Surgeon" },
                new VetSpecialization { Name = "Dermatologist" },
                new VetSpecialization { Name = "Cardiologist" },
                new VetSpecialization { Name = "Orthopedist" },
                new VetSpecialization { Name = "Ophthalmologist" },
                new VetSpecialization { Name = "Anesthesiologist" },
                new VetSpecialization { Name = "Radiologist" },
                new VetSpecialization { Name = "Oncologist" },
                new VetSpecialization { Name = "Neurologist" }
            };

            await context.VetSpecializations.AddRangeAsync(specializations);

            await context.SaveChangesAsync();
        }
    }
}

