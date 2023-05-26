using FluentValidation;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Persistence.Repositories.ProfileImageRepository;
using Idt.Profiles.Shared.ConfigurationOptions;
using Idt.Profiles.Shared.Exceptions.SystemCriticalExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Idt.Profiles.Services.ProfileImageService.Implementations;

public class LocalDriveProfileImageService : IProfileImageService
{
    private readonly LocalDriveImageStorageOptions _storageOptions;
    private readonly IProfileImageInfoRepository _imageInfoRepository;
    private readonly IValidator<IFormFile> _imageValidator;
    
    private static void CreateImageDirectoryIfNeeded(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
    }

    public LocalDriveProfileImageService(IOptions<LocalDriveImageStorageOptions> storageOptions,
        IProfileImageInfoRepository imageInfoRepository, IValidator<IFormFile> imageValidator)
    {
        _imageInfoRepository = imageInfoRepository;
        _imageValidator = imageValidator;
        _storageOptions = storageOptions.Value;
        CreateImageDirectoryIfNeeded(_storageOptions.ImageDirectoryName);
    }

    public async Task<(MemoryStream FileContent, string FileType)> GetProfileImageAsync(Guid profileId)
    {
        string filePath;
        string fileType;
        MemoryStream fileContent = new MemoryStream();

        var savedImageInfo = await _imageInfoRepository.GetProfileImageInfoAsync(profileId);
        if (savedImageInfo is not null)
        {
            fileType = savedImageInfo.FileType;
            filePath = BuildFilePath(_storageOptions.ImageDirectoryName, savedImageInfo.FileName);
        }
        else
        {
            filePath = BuildFilePath(_storageOptions.ImageDirectoryName, _storageOptions.DefaultProfileImageName);
            fileType = _storageOptions.DefaultProfileImageType;
        }

        if (!File.Exists(filePath))
        {
            throw new FailedToGetProfileImageFromDriveException(
                $"Failed to get image at the following path: {filePath}. " +
                $"Either the image for profile with id {profileId} has been deleted with errors " +
                $"or default profile image has been moved.");
        }

        await using var file = File.OpenRead(filePath);
        await file.CopyToAsync(fileContent);
        fileContent.Position = 0;
        return new(fileContent, fileType);
    }


    public async Task UpdateProfileImageAsync(Guid profileId, IFormFile image)
    {
        var profileImageName = profileId.ToString();
        var imageExists = CheckIfProfileImageExists(profileImageName, out var filePath);
        if (imageExists)
        {
            DeleteProfileImage(profileId);
        }

        _imageValidator.ValidateAndThrow(image);
        await WriteImageToLocalDriveAsync(filePath, image);
        await _imageInfoRepository.SaveProfileImageInfoAsync(new ProfileImageInfo
        {
            ProfileId = profileId,
            FileName = profileId.ToString(),
            FileType = image.ContentType
        });
    }

    public void DeleteProfileImage(Guid profileId)
    {
        _imageInfoRepository.DeleteProfileImageInfoAsync(profileId);
    }

    private static string BuildFilePath(string imagesDirectoryName, string fileName) =>
        Path.Combine(imagesDirectoryName, fileName);
    
    private async Task WriteImageToLocalDriveAsync(string filePath, IFormFile image)
    {
        await using var file = File.OpenWrite(filePath);
        await image.CopyToAsync(file);
        await file.FlushAsync();
    }

    private bool CheckIfProfileImageExists(string imageFileName, out string filePath)
    {
        filePath = BuildFilePath(_storageOptions.ImageDirectoryName, imageFileName);
        return File.Exists(filePath);
    }
}