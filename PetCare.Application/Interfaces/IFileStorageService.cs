using Microsoft.AspNetCore.Http;

namespace PetCare.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string subFolder);

        Task<byte[]> GetFileAsync(string fileName, string subFolder);

        void DeleteFile(string fileName, string subFolder);
    }
}
