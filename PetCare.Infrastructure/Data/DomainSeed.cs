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
                new VetSpecialization { Name = "Ogólna" },
                new VetSpecialization { Name = "Internista" },
                new VetSpecialization { Name = "Chirurg" },
                new VetSpecialization { Name = "Dermatolog" },
                new VetSpecialization { Name = "Kardiolog" },
                new VetSpecialization { Name = "Ortopeda" },
                new VetSpecialization { Name = "Okulista" },
                new VetSpecialization { Name = "Anestezjolog" },
                new VetSpecialization { Name = "Radiolog" },
                new VetSpecialization { Name = "Onkolog" },
                new VetSpecialization { Name = "Neurolog" }
            };

            await context.VetSpecializations.AddRangeAsync(specializations);

            await context.SaveChangesAsync();
        }
    }
}

