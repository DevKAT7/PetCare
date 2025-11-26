using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class AppointmentProcedure
    {
        // add later in DbContext with Fluent API:
        // builder.Entity<AppointmentProcedure>().HasKey(ap => new { ap.AppointmentId, ap.ProcedureId });

        [Required]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; } = null!;

        [Required]
        public int ProcedureId { get; set; }
        public virtual Procedure Procedure { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Cena nie może być ujemna.")]
        public decimal FinalPrice { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Ilość musi być większa od zera.")]
        public int Quantity { get; set; } = 1;

        [NotMapped]
        public decimal TotalPrice => FinalPrice * Quantity;
    }
}

