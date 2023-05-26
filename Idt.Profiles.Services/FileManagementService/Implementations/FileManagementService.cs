using Idt.Profiles.Shared.Exceptions.SystemCriticalExceptions;
using Microsoft.AspNetCore.Http;

namespace Idt.Profiles.Services.FileManagementService.Implementations;

public class FileManagementService : IFileManagementService
{
    public async Task<MemoryStream> GetFileContent(string filePath)
    {
        var fileContent = new MemoryStream();

        if (!File.Exists(filePath))
        {
            throw new FailedToGetProfileImageFromDriveException(
                $"Failed to get image at the following path: {filePath}. " +
                $"Either the image for profile has been deleted with errors " +
                $"or default profile image has been moved.");
        }

        await using var file = File.OpenRead(filePath);
        await file.CopyToAsync(fileContent);
        fileContent.Position = 0;
        return fileContent;
    }

    public async Task WriteImageToLocalDriveAsync(string filePath, IFormFile image)
    {
        await using var file = File.OpenWrite(filePath);
        await image.CopyToAsync(file);
        await file.FlushAsync();
    }

    public bool CheckIfFileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public string BuildFilePath(string directoryPath, string fileName)
    {
        return Path.Combine(directoryPath, fileName);
    }
}