using Microsoft.AspNetCore.Http;

namespace Idt.Profiles.Services.FileManagementService;

public interface IFileManagementService
{
    Task<MemoryStream> GetFileContent(string filePath);
    Task WriteImageToLocalDriveAsync(string filePath, IFormFile file);
    bool CheckIfFileExists(string filePath);
    string BuildFilePath(string directoryPath, string fileName);

}