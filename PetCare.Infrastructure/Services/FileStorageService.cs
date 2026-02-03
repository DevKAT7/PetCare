using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PetCare.Application.Interfaces;

namespace PetCare.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _baseStoragePath;

        public FileStorageService(IConfiguration configuration)
        {
            var folderName = configuration["StorageSettings:FolderName"] ?? "PetCare_Uploads";

            var currentDir = Directory.GetCurrentDirectory();

            _baseStoragePath = Path.GetFullPath(Path.Combine(currentDir, "..", folderName));

            if (!Directory.Exists(_baseStoragePath))
            {
                Directory.CreateDirectory(_baseStoragePath);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0) return null;

            var targetFolder = Path.Combine(_baseStoragePath, subFolder);
            if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

            var fileName = Path.GetFileName(file.FileName);
            var safeFileName = fileName.Replace(" ", "_").Replace("(", "").Replace(")", "");
            var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";

            var fullPath = Path.Combine(targetFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public async Task<byte[]> GetFileAsync(string fileName, string subFolder)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            var filePath = Path.Combine(_baseStoragePath, subFolder, fileName);

            if (!File.Exists(filePath)) return null;

            return await File.ReadAllBytesAsync(filePath);
        }

        public void DeleteFile(string fileName, string subFolder)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            var filePath = Path.Combine(_baseStoragePath, subFolder, fileName);
            if (File.Exists(filePath)) File.Delete(filePath);
        }
    }
}
