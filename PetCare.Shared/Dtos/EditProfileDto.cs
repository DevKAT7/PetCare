using System.ComponentModel.DataAnnotations;

namespace PetCare.Shared.Dtos
{
    public class EditProfileDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(20, MinimumLength = 9, ErrorMessage = "Phone number must be between 9 and 20 characters.")]
        [RegularExpression("^[0-9+() -]*$", ErrorMessage = "Invalid phone number format. Allowed: digits, +, -, spaces, ().")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot be longer than 500 characters.")]
        public string Address { get; set; } = string.Empty;
    }
}
