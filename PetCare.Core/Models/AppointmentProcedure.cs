using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Core.Models
{
    public class AppointmentProcedure
    {
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; } = null!;
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "The price cannot be negative.")]
        public decimal FinalPrice { get; set; }
        [Range(1, 100, ErrorMessage = "The quantity must be greater than zero.")]
        public int Quantity { get; set; } = 1;
        [NotMapped]
        public decimal TotalPrice => FinalPrice * Quantity;
    }
}

