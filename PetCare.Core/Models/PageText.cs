using System.ComponentModel.DataAnnotations;

namespace PetCare.Core.Models
{
    public class PageText
    {
        [Key]
        public int PageTextId { get; set; }

        [MaxLength(50)]
        public string Key { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
