namespace PetCare.MobileApp.Models.Pets
{
    public class PetDetailDto
    {
        public int PetId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string? Breed { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string? ImageUrl { get; set; }

        //owner details
        public int PetOwnerId { get; set; }
        public string OwnerFullName { get; set; } = string.Empty;
        public string OwnerPhoneNumber { get; set; } = string.Empty;
        public string OwnerEmail { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public List<PetAppointmentDto> Appointments { get; set; } = new();
        public List<PetPrescriptionDto> Prescriptions { get; set; } = new();
        public List<PetTestDto> MedicalTests { get; set; } = new();
        public List<PetVaccinationDto> Vaccinations { get; set; } = new();

        public int Age => CalculateAge(DateOfBirth);
        private static int CalculateAge(DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    public class PetAppointmentDto
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; } = string.Empty;
        public string VetName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class PetPrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public DateOnly Date { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public int AppointmentId { get; set; }
    }

    public class PetTestDto
    {
        public int MedicalTestId { get; set; }
        public DateOnly Date { get; set; }
        public string TestName { get; set; } = string.Empty;
        public string? Result { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
    }

    public class PetVaccinationDto
    {
        public int VaccinationId { get; set; }
        public string VaccineName { get; set; } = string.Empty;
        public DateOnly DateAdministered { get; set; }
        public DateOnly? NextDueDate { get; set; }
    }
}
