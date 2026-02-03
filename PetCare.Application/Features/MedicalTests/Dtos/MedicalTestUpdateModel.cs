using Microsoft.AspNetCore.Http;

namespace PetCare.Application.Features.MedicalTests.Dtos
{
    public class MedicalTestUpdateModel
    {
        public string TestName { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
        public IFormFile? AttachmentFile { get; set; }
    }
}
