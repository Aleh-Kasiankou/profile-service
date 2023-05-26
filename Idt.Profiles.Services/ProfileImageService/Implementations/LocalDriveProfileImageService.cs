using FluentValidation;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Persistence.Repositories.ProfileImageRepository;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository;
using Idt.Profiles.Services.FileManagementService;
using Idt.Profiles.Shared.ConfigurationOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Idt.Profiles.Services.ProfileImageService.Implementations;

public class LocalDriveProfileImageService : IProfileImageService
{
    private readonly LocalDriveImageStorageOptions _storageOptions;
    private readonly IFileManagementService _fileManagementService;
    private readonly IProfileRepository _profileRepository;
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
        IProfileImageInfoRepository imageInfoRepository, IValidator<IFormFile> imageValidator,
        IProfileRepository profileRepository, IFileManagementService fileManagementService)
    {
        _imageInfoRepository = imageInfoRepository;
        _imageValidator = imageValidator;
        _profileRepository = profileRepository;
        _fileManagementService = fileManagementService;
        _storageOptions = storageOptions.Value;
        CreateImageDirectoryIfNeeded(_storageOptions.ImageDirectoryName);
    }

    public async Task<(MemoryStream FileContent, string FileType)> GetProfileImageAsync(Guid profileId)
    {
        string filePath;
        string fileType;

        var savedImageInfo = await _imageInfoRepository.GetProfileImageInfoAsync(profileId);
        if (savedImageInfo is not null)
        {
            fileType = savedImageInfo.FileType;
            filePath = _fileManagementService.BuildFilePath(_storageOptions.ImageDirectoryName,
                savedImageInfo.FileName);
        }
        else
        {
            filePath = _fileManagementService.BuildFilePath(_storageOptions.ImageDirectoryName,
                _storageOptions.DefaultProfileImageName);
            fileType = _storageOptions.DefaultProfileImageType;
        }

        var fileContent = await _fileManagementService.GetFileContent(filePath);
        return new(fileContent, fileType);
    }


    public async Task UpdateProfileImageAsync(Guid profileId, IFormFile image)
    {
        await _profileRepository.GetProfileAsync(profileId);
        var profileImagePath =
            _fileManagementService.BuildFilePath(_storageOptions.ImageDirectoryName, profileId.ToString());
        var imageExists = _fileManagementService.CheckIfFileExists(profileImagePath);
        if (imageExists)
        {
            DeleteProfileImage(profileId);
        }

        _imageValidator.ValidateAndThrow(image);
        await _fileManagementService.WriteImageToLocalDriveAsync(profileImagePath, image);
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
}