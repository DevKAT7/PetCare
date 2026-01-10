using PetCare.Application.Features.Prescriptions.Dtos;

namespace PetCare.Application.Interfaces
{
    public interface IDocumentGenerator
    {
        (byte[] Content, string ContentType, string FileName) GeneratePrescription(PrescriptionReadModel data, string templateId);
    }
}
