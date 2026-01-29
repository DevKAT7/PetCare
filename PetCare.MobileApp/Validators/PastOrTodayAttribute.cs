using System.ComponentModel.DataAnnotations;

namespace PetCare.MobileApp.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PastOrTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (value is DateTime dt)
            {
                if (dt.Date > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage ?? "Date cannot be in the future.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
