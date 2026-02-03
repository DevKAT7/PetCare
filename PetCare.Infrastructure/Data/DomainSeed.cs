using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare.Core.Enums;
using PetCare.Core.Models;

namespace PetCare.Infrastructure.Data
{
    public static class DomainSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<User> userManager)
        {
            await SeedSpecializationsAsync(context);
            await SeedProceduresAsync(context);
            await SeedVetsAsync(context, userManager);
            await SeedSchedulesAsync(context);
            await SeedMedicationsAsync(context);
            await SeedClientsAndPetsAsync(context, userManager);
            await SeedScenariosAsync(context);
        }

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

        private static async Task SeedProceduresAsync(ApplicationDbContext context)
        {
            if (await context.Procedures.AnyAsync()) return;

            var genId = (await context.VetSpecializations.FirstAsync(s => s.Name == "General Practitioner")).VetSpecializationId;
            var internId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Internist")).VetSpecializationId;
            var surgId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Surgeon")).VetSpecializationId;
            var dermId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Dermatologist")).VetSpecializationId;
            var cardioId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Cardiologist")).VetSpecializationId;
            var orthoId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Orthopedist")).VetSpecializationId;
            var ophthId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Ophthalmologist")).VetSpecializationId;
            var radioId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Radiologist")).VetSpecializationId;
            var oncoId = (await context.VetSpecializations.FirstAsync(s => s.Name == "Oncologist")).VetSpecializationId;

            var procedures = new List<Procedure>
            {
                new() { Name = "General Check-up", Description = "Comprehensive health examination.", Price = 100, IsActive = true, VetSpecializationId = genId },

                new() { Name = "Follow-up Visit", Description = "Short check-up to monitor treatment progress.", Price = 80, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Vaccination Visit", Description = "Health check before vaccination + injection.", Price = 50, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Emergency Visit", Description = "Urgent care outside of regular schedule/hours.", Price = 300, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Nail Trimming", Description = "Hygienic shortening of claws.", Price = 30, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Ear Cleaning", Description = "Ear hygiene and medication administration.", Price = 40, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Anal Gland Expression", Description = "Mechanical emptying of anal glands.", Price = 40, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Tick Removal", Description = "Parasite removal and site disinfection.", Price = 20, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Microchipping + Registration", Description = "ISO microchip implantation and database entry.", Price = 100, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Injection", Description = "Subcutaneous or intramuscular drug administration.", Price = 25, IsActive = true, VetSpecializationId = genId },
                new() { Name = "IV Cannula Placement", Description = "Venous catheter placement.", Price = 35, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Pet Passport Issue", Description = "Issuing an EU passport for travel.", Price = 100, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Deworming", Description = "Administration of antiparasitic medication.", Price = 20, IsActive = true, VetSpecializationId = genId },
        
                new() { Name = "Bloodwork (Morphology)", Description = "Basic blood count analysis.", Price = 40, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Bloodwork (Biochemistry)", Description = "Organ function analysis (Kidney/Liver profile).", Price = 90, IsActive = true, VetSpecializationId = genId },
                new() { Name = "Fecal Analysis", Description = "Microscope test for parasites.", Price = 50, IsActive = true, VetSpecializationId = genId },

                new() { Name = "Internal Medicine Consult", Description = "Extended diagnostics of internal diseases.", Price = 180, IsActive = true, VetSpecializationId = internId },
                new() { Name = "IV Drip Therapy", Description = "Intravenous fluid administration.", Price = 60, IsActive = true, VetSpecializationId = internId },

                new() { Name = "Cat Castration", Description = "Male cat neutering under general anesthesia.", Price = 250, IsActive = true, VetSpecializationId = surgId },
                new() { Name = "Cat Spay (Sterilization)", Description = "Female cat surgery (small incision technique).", Price = 450, IsActive = true, VetSpecializationId = surgId },
                new() { Name = "Wound Suturing", Description = "Surgical management of traumatic wounds.", Price = 150, IsActive = true, VetSpecializationId = surgId },
                new() { Name = "Dental Cleaning (Scaling)", Description = "Ultrasound tartar removal and polishing.", Price = 350, IsActive = true, VetSpecializationId = surgId },
                new() { Name = "Dog Castration (<10kg)", Description = "Neutering for small male dogs.", Price = 350, IsActive = true, VetSpecializationId = surgId },
                new() { Name = "Dog Spay (<10kg)", Description = "Sterilization for small female dogs.", Price = 500, IsActive = true, VetSpecializationId = surgId },

                new() { Name = "Abdominal Ultrasound", Description = "Full ultrasound imaging of the abdomen.", Price = 180, IsActive = true, VetSpecializationId = radioId },
                new() { Name = "General X-Ray", Description = "X-Ray imaging (1 projection).", Price = 100, IsActive = true, VetSpecializationId = radioId },

                new() { Name = "Dermatology Consult", Description = "Skin examination, smears, and scrapings.", Price = 200, IsActive = true, VetSpecializationId = dermId },
                new() { Name = "Cytology", Description = "Microscopic examination of cells/smears.", Price = 60, IsActive = true, VetSpecializationId = dermId },
                new() { Name = "Skin Allergy Test", Description = "Dermatological scraping and allergy analysis.", Price = 100, IsActive = true, VetSpecializationId = dermId },

                new() { Name = "Cardiology Consult + Echo", Description = "Heart examination including Echocardiogram.", Price = 250, IsActive = true, VetSpecializationId = cardioId },

                new() { Name = "Orthopedic Consult", Description = "Mobility assessment and lameness diagnosis.", Price = 220, IsActive = true, VetSpecializationId = orthoId },

                new() { Name = "Ophthalmology Consult", Description = "Comprehensive eye examination.", Price = 200, IsActive = true, VetSpecializationId = ophthId },

                new() { Name = "Oncology Consult", Description = "Cancer treatment planning and chemotherapy.", Price = 220, IsActive = true, VetSpecializationId = oncoId }
            };

            await context.Procedures.AddRangeAsync(procedures);
            await context.SaveChangesAsync();
        }

        private static async Task SeedVetsAsync(ApplicationDbContext context, UserManager<User> userManager)
        {
            if (await context.Vets.AnyAsync()) return;

            var surgSpec = await context.VetSpecializations.FirstAsync(s => s.Name == "Surgeon");
            var dermSpec = await context.VetSpecializations.FirstAsync(s => s.Name == "Dermatologist");
            var genSpec = await context.VetSpecializations.FirstAsync(s => s.Name == "General Practitioner");

            var user1 = new User
            {
                UserName = "john.doe@petcare.com",
                Email = "john.doe@petcare.com",
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumber = "+44 7700 900001"
            };

            if (await userManager.FindByEmailAsync(user1.Email) == null)
            {
                await userManager.CreateAsync(user1, "Pass123!");
                await userManager.AddToRoleAsync(user1, IdentitySeed.RoleEmployee);

                var vet1 = new Vet
                {
                    UserId = user1.Id,
                    FirstName = "John",
                    LastName = "Doe",
                    Description = "Senior Surgeon specializing in orthopedic and soft tissue surgeries. With over 10 years of experience, John ensures every procedure is performed with the highest precision and care.",
                    CareerStartDate = new DateOnly(2014, 5, 1),
                    HireDate = new DateOnly(2020, 1, 15),
                    Address = "123 Baker Street, London",
                    Pesel = "85051512345",
                    LicenseNumber = "VET-882910",
                    IsActive = true,
                    ProfilePictureUrl = "https://randomuser.me/api/portraits/men/32.jpg"
                };
                context.Vets.Add(vet1);
                context.VetSpecializationLinks.Add(new VetSpecializationLink { Vet = vet1, VetSpecialization = surgSpec });
            }

            var user2 = new User
            {
                UserName = "sarah.smith@petcare.com",
                Email = "sarah.smith@petcare.com",
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumber = "+44 7700 900002"
            };

            if (await userManager.FindByEmailAsync(user2.Email) == null)
            {
                await userManager.CreateAsync(user2, "Pass123!");
                await userManager.AddToRoleAsync(user2, IdentitySeed.RoleEmployee);

                var vet2 = new Vet
                {
                    UserId = user2.Id,
                    FirstName = "Sarah",
                    LastName = "Smith",
                    Description = "Passionate Dermatologist dedicated to solving complex skin allergies and ear infections. Sarah loves helping pets feel comfortable in their own skin again.",
                    CareerStartDate = new DateOnly(2018, 9, 1),
                    HireDate = new DateOnly(2021, 3, 10),
                    Address = "45 Oxford Street, London",
                    Pesel = "92090154321",
                    LicenseNumber = "VET-443211",
                    IsActive = true,
                    ProfilePictureUrl = "https://randomuser.me/api/portraits/women/44.jpg"
                };
                context.Vets.Add(vet2);
                context.VetSpecializationLinks.Add(new VetSpecializationLink { Vet = vet2, VetSpecialization = dermSpec });
            }

            var user3 = new User
            {
                UserName = "emily.white@petcare.com",
                Email = "emily.white@petcare.com",
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumber = "+44 7700 900003"
            };

            if (await userManager.FindByEmailAsync(user3.Email) == null)
            {
                await userManager.CreateAsync(user3, "Pass123!");
                await userManager.AddToRoleAsync(user3, IdentitySeed.RoleEmployee);

                var vet3 = new Vet
                {
                    UserId = user3.Id,
                    FirstName = "Emily",
                    LastName = "White",
                    Description = "Friendly and energetic General Practitioner. Emily focuses on preventive care, vaccinations, and nutrition advice to keep your furry friends healthy.",
                    CareerStartDate = new DateOnly(2023, 6, 1),
                    HireDate = new DateOnly(2023, 7, 1),
                    Address = "88 Queen's Road, Bristol",
                    Pesel = "99060198765",
                    LicenseNumber = "VET-110022",
                    IsActive = true,
                    ProfilePictureUrl = "https://randomuser.me/api/portraits/women/68.jpg"
                };
                context.Vets.Add(vet3);
                context.VetSpecializationLinks.Add(new VetSpecializationLink { Vet = vet3, VetSpecialization = genSpec });
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedSchedulesAsync(ApplicationDbContext context)
        {
            if (await context.VetSchedules.AnyAsync()) return;

            var john = await context.Vets.FirstOrDefaultAsync(v => v.FirstName == "John" && v.LastName == "Doe");
            var sarah = await context.Vets.FirstOrDefaultAsync(v => v.FirstName == "Sarah" && v.LastName == "Smith");
            var emily = await context.Vets.FirstOrDefaultAsync(v => v.FirstName == "Emily" && v.LastName == "White");

            var schedules = new List<VetSchedule>();

            if (john != null)
            {
                schedules.Add(new VetSchedule { VetId = john.VetId, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) });
                schedules.Add(new VetSchedule { VetId = john.VetId, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) });
                schedules.Add(new VetSchedule { VetId = john.VetId, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(14, 0) });
                schedules.Add(new VetSchedule { VetId = john.VetId, DayOfWeek = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) });
            }

            if (sarah != null)
            {
                schedules.Add(new VetSchedule { VetId = sarah.VetId, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(17, 0) });
                schedules.Add(new VetSchedule { VetId = sarah.VetId, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(17, 0) });
                schedules.Add(new VetSchedule { VetId = sarah.VetId, DayOfWeek = DayOfWeek.Thursday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(17, 0) });
                schedules.Add(new VetSchedule { VetId = sarah.VetId, DayOfWeek = DayOfWeek.Friday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(15, 0) });
            }

            if (emily != null)
            {
                schedules.Add(new VetSchedule { VetId = emily.VetId, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(16, 0) });
                schedules.Add(new VetSchedule { VetId = emily.VetId, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0) });
                schedules.Add(new VetSchedule { VetId = emily.VetId, DayOfWeek = DayOfWeek.Friday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(16, 0) });
            }

            await context.VetSchedules.AddRangeAsync(schedules);
            await context.SaveChangesAsync();
        }

        private static async Task SeedMedicationsAsync(ApplicationDbContext context)
        {
            if (await context.Medications.AnyAsync()) return;

            var meds = new List<Medication>
            {
                new() { Name = "Amoxicillin 250mg", Description = "Antibiotic.", Manufacturer = "VetPharma", Price = 15.00m, IsActive = true },
                new() { Name = "Meloxicam", Description = "Pain relief (NSAID).", Manufacturer = "PainAway", Price = 25.50m, IsActive = true },
                new() { Name = "Apoquel 16mg", Description = "Itch relief for allergies.", Manufacturer = "Zoetis", Price = 80.00m, IsActive = true },
                new() { Name = "Drontal Plus", Description = "Deworming tablets.", Manufacturer = "Bayer", Price = 12.00m, IsActive = true }
            };
            await context.Medications.AddRangeAsync(meds);
            await context.SaveChangesAsync();

            var stockItems = meds.Select(m => new StockItem { Medication = m, CurrentStock = 50, ReorderLevel = 10 }).ToList();
            await context.StockItems.AddRangeAsync(stockItems);
            await context.SaveChangesAsync();
        }

        private static async Task SeedClientsAndPetsAsync(ApplicationDbContext context, UserManager<User> userManager)
        {
            if (await context.PetOwners.AnyAsync()) return;

            var alice = await CreateClient(context, userManager, "alice@petcare.com", "Alice", "Wonder", "829371625");
            context.Pets.Add(new Pet 
            { 
                Name = "Buddy", 
                Species = "Dog", 
                Breed = "Golden Retriever", 
                DateOfBirth = new DateOnly(2020, 5, 20), 
                IsMale = true, 
                IsActive = true, 
                PetOwnerId = alice.PetOwnerId, 
                ImageUrl = "https://images.pexels.com/photos/2253275/pexels-photo-2253275.jpeg" 
            });

            var bob = await CreateClient(context, userManager, "bob@petcare.com", "Bob", "Builder", "837884992");
            context.Pets.Add(new Pet 
            { 
                Name = "Whiskers", 
                Species = "Cat", 
                Breed = "Persian", 
                DateOfBirth = new DateOnly(2019, 2, 10), 
                IsMale = false, 
                IsActive = true, 
                PetOwnerId = bob.PetOwnerId, 
                ImageUrl = "https://images.pexels.com/photos/45201/kitty-cat-kitten-pet-45201.jpeg" 
            });

            await context.SaveChangesAsync();
        }

        private static async Task<PetOwner> CreateClient(ApplicationDbContext context, 
            UserManager<User> userManager, string email, string fName, string lName, string phoneNumber)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                IsActive = true,
                PhoneNumber = phoneNumber

            };

            var result = await userManager.CreateAsync(user, "Pass123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, IdentitySeed.RoleClient);

                var owner = new PetOwner
                {
                    UserId = user.Id,
                    FirstName = fName,
                    LastName = lName,
                    Address = "Test St. 123",
                    PhoneNumber = phoneNumber,
                    IsActive = true
                };

                context.PetOwners.Add(owner);
                await context.SaveChangesAsync();
                return owner;
            }
            throw new Exception("Error creating user");
        }

        private static async Task SeedScenariosAsync(ApplicationDbContext context)
        {
            if (await context.Appointments.AnyAsync()) return;

            var vetGP = await context.Vets.FirstAsync(v => v.FirstName == "Emily");
            var vetDerm = await context.Vets.FirstAsync(v => v.FirstName == "Sarah");
            var vetSurg = await context.Vets.FirstAsync(v => v.FirstName == "John");

            var buddy = await context.Pets.Include(p => p.PetOwner).FirstAsync(p => p.Name == "Buddy");
            var whiskers = await context.Pets.Include(p => p.PetOwner).FirstAsync(p => p.Name == "Whiskers");

            var procCheckup = await context.Procedures.FirstAsync(p => p.Name == "General Check-up");
            var procVaccine = await context.Procedures.FirstAsync(p => p.Name == "Vaccination Visit");
            var procAllergy = await context.Procedures.FirstAsync(p => p.Name == "Skin Allergy Test");
            var procMicrochip = await context.Procedures.FirstAsync(p => p.Name.Contains("Microchip"));

            var medApoquel = await context.Medications.FirstAsync(m => m.Name.Contains("Apoquel"));
            var medDrontal = await context.Medications.FirstAsync(m => m.Name.Contains("Drontal"));

            // SCENARIUSZ A: HISTORIA (Wizyta zakończona, opłacona, zdrowy pies)
            // Pies Buddy był 2 tygodnie temu u Emily na przeglądzie i szczepieniu.
            var dateA = GetPastDate(DayOfWeek.Wednesday, weeksAgo: 2);

            var app1 = new Appointment
            {
                PetId = buddy.PetId,
                VetId = vetGP.VetId,
                AppointmentDateTime = dateA.AddHours(12),
                Status = AppointmentStatus.Completed,
                ReasonForVisit = "Annual wellness exam and vaccination",
                Diagnosis = "Clinically healthy. Good body condition score.",
                Notes = "Patient is very friendly. Loves treats.",
                IsReminderSent = true
            };
            context.Appointments.Add(app1);
            await context.SaveChangesAsync();

            // Pozycje na fakturze / Wykonane procedury
            // (Zakładam, że masz tabelę AppointmentProcedures lub InvoiceItems - dostosuj wg modelu)
            context.AppointmentProcedures.AddRange(
                new AppointmentProcedure { AppointmentId = app1.AppointmentId, ProcedureId = procCheckup.ProcedureId, Quantity = 1, FinalPrice = procCheckup.Price },
                new AppointmentProcedure { AppointmentId = app1.AppointmentId, ProcedureId = procVaccine.ProcedureId, Quantity = 1, FinalPrice = procVaccine.Price }
            );

            // Faktura do wizyty A (Opłacona)
            var inv1 = new Invoice
            {
                AppointmentId = app1.AppointmentId,
                PetOwnerId = buddy.PetOwnerId,
                InvoiceNumber = "INV-2026-001",
                InvoiceDate = DateOnly.FromDateTime(app1.AppointmentDateTime),
                DueDate = DateOnly.FromDateTime(app1.AppointmentDateTime.AddDays(7)),
                TotalAmount = procCheckup.Price + procVaccine.Price,
                IsPaid = true,
                PaymentDate = DateOnly.FromDateTime(app1.AppointmentDateTime)
            };
            context.Invoices.Add(inv1);
            await context.SaveChangesAsync();

            context.InvoiceItems.AddRange(
                new InvoiceItem { InvoiceId = inv1.InvoiceId, Description = procCheckup.Name, Quantity = 1, UnitPrice = procCheckup.Price, LineTotal = procCheckup.Price },
                new InvoiceItem { InvoiceId = inv1.InvoiceId, Description = procVaccine.Name, Quantity = 1, UnitPrice = procVaccine.Price, LineTotal = procVaccine.Price }
            );

            // SCENARIUSZ B: PROBLEM (Wizyta zakończona, NIEOPŁACONA - OVERDUE, Leki)
            // Kot Whiskers miał alergię miesiąc temu u Sarah. Właściciel Bob nie zapłacił.
            var dateB = GetPastDate(DayOfWeek.Tuesday, weeksAgo: 5);

            var app2 = new Appointment
            {
                PetId = whiskers.PetId,
                VetId = vetDerm.VetId,
                AppointmentDateTime = dateB.AddHours(11),
                Status = AppointmentStatus.Completed,
                ReasonForVisit = "Constant scratching and hair loss",
                Diagnosis = "Atopic Dermatitis (Allergy)",
                Notes = "Prescribed Apoquel. Dietary change recommended.",
                IsReminderSent = true
            };
            context.Appointments.Add(app2);
            await context.SaveChangesAsync();

            context.AppointmentProcedures.Add(new AppointmentProcedure { AppointmentId = app2.AppointmentId, ProcedureId = procAllergy.ProcedureId, Quantity = 1, FinalPrice = procAllergy.Price });

            // Recepta do wizyty B
            context.Prescriptions.Add(new Prescription
            {
                AppointmentId = app2.AppointmentId,
                MedicationId = medApoquel.MedicationId,
                Dosage = "1 tablet daily",
                Frequency = "Once a day",
                StartDate = DateOnly.FromDateTime(app2.AppointmentDateTime),
                EndDate = DateOnly.FromDateTime(app2.AppointmentDateTime.AddDays(14)),
                Instructions = "Give with food. Do not stop abruptly.",
                PacksToDispense = 1,
                CreatedDate = DateOnly.FromDateTime(app2.AppointmentDateTime)
            });

            // Faktura do wizyty B (NIEOPŁACONA i PRZETERMINOWANA)
            var inv2 = new Invoice
            {
                AppointmentId = app2.AppointmentId,
                PetOwnerId = whiskers.PetOwnerId,
                InvoiceNumber = "INV-2026-002",
                InvoiceDate = DateOnly.FromDateTime(app2.AppointmentDateTime),
                DueDate = DateOnly.FromDateTime(app2.AppointmentDateTime.AddDays(7)),
                TotalAmount = procAllergy.Price + medApoquel.Price,
                IsPaid = false
            };
            context.Invoices.Add(inv2);
            await context.SaveChangesAsync();

            context.InvoiceItems.AddRange(
                new InvoiceItem { InvoiceId = inv2.InvoiceId, Description = procAllergy.Name, Quantity = 1, UnitPrice = procAllergy.Price, LineTotal = procAllergy.Price },
                new InvoiceItem { InvoiceId = inv2.InvoiceId, Description = medApoquel.Name, Quantity = 1, UnitPrice = medApoquel.Price, LineTotal = medApoquel.Price }
            );

            // SCENARIUSZ C: PRZYSZŁOŚĆ (Zaplanowana wizyta)
            // Buddy idzie jutro na czipowanie do Johna.
            var dateC = GetFutureDate(DayOfWeek.Monday);

            var app3 = new Appointment
            {
                PetId = buddy.PetId,
                VetId = vetSurg.VetId,
                AppointmentDateTime = dateC.AddHours(10),
                Status = AppointmentStatus.Scheduled,
                ReasonForVisit = "Microchipping request",
                IsReminderSent = false
            };
            context.Appointments.Add(app3);


            // SCENARIUSZ D: ANULOWANA
            // Whiskers miał mieć kontrolę, ale Bob odwołał.
            var dateD = GetPastDate(DayOfWeek.Friday, weeksAgo: 0);

            var app4 = new Appointment
            {
                PetId = whiskers.PetId,
                VetId = vetDerm.VetId,
                AppointmentDateTime = dateD.AddHours(11),
                Status = AppointmentStatus.Cancelled,
                ReasonForVisit = "Allergy follow-up",
                Notes = "Owner cancelled via app."
            };
            context.Appointments.Add(app4);

            await context.SaveChangesAsync();
        }
        private static DateTime GetFutureDate(DayOfWeek day)
        {
            var date = DateTime.Now.Date.AddDays(1);
            while (date.DayOfWeek != day)
            {
                date = date.AddDays(1);
            }

            return date;
        }

        private static DateTime GetPastDate(DayOfWeek day, int weeksAgo)
        {
            var date = DateTime.Now.Date.AddDays(-1);
            while (date.DayOfWeek != day)
            {
                date = date.AddDays(-1);
            }

            return date.AddDays(-7 * weeksAgo);
        }
    }
}

