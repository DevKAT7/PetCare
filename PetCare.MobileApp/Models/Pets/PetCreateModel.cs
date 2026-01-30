using PetCare.MobileApp.Validators;
using System.ComponentModel.DataAnnotations;

namespace PetCare.MobileApp.Models.Pets
{
    public class PetCreateModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Species is required.")]
        [StringLength(50, ErrorMessage = "Species cannot exceed 50 characters.")]
        public string Species { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Breed cannot exceed 50 characters.")]
        public string? Breed { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [PastOrToday(ErrorMessage = "Date of birth cannot be in the future.")]
        public DateTime DateOfBirth { get; set; }

        public bool IsMale { get; set; }

        [Url(ErrorMessage = "Invalid image URL.")]
        public string? ImageUrl { get; set; }

        public int PetOwnerId { get; set; }
    }
}
