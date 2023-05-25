using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Persistence.Repositories.ProfileImageRepository;
using Idt.Profiles.Shared.ConfigurationOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Idt.Profiles.Services.ProfileImageService.Implementations;

// TODO CHECK RECOMMENDED METHOD PROPS ORDER
public class LocalDriveProfileImageService : IProfileImageService
{
    private readonly LocalDriveImageStorageOptions _storageOptions;
    private readonly IProfileImageInfoRepository _imageInfoRepository;

    // TODO MAKE SURE EXCEPTION DOESN'T CAUSE ISSUES
    // TODO CREATE DISTINCT DBSET WITH IMAGES AND USE REPO HERE, CASCADE DELETE THEM WITHIN PROFILE IF NEEDED
    // TODO CHECK IF WE SHOULD LIMIT FILE EXTENSIONS

    private static string BuildFilePath(string imagesDirectoryName, string fileName) =>
        Path.Combine(imagesDirectoryName, fileName);

    private static void CreateImageDirectoryIfNeeded(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
    }

    public LocalDriveProfileImageService(IOptions<LocalDriveImageStorageOptions> storageOptions,
        IProfileImageInfoRepository imageInfoRepository)
    {
        _imageInfoRepository = imageInfoRepository;
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
            // TODO USE CONFIG
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./blank-profile-picture.webp");
            fileType = "image/webp";
        }

        if (!File.Exists(filePath))
        {
            //TODO throw exception
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

        // TODO USE METHODS?
        // writing to drive
        await using var file = File.OpenWrite(filePath);
        await image.CopyToAsync(file);
        await file.FlushAsync();
        
        // TODO VALIDATE WHETHER IMAGE

        // writing to database
        var profileImageInfo = new ProfileImageInfo
        {
            ProfileId = profileId,
            FileName = profileId.ToString(),
            FileType = image.ContentType
        };
        

        await _imageInfoRepository.SaveProfileImageInfoAsync(profileImageInfo);
    }

    public void DeleteProfileImage(Guid profileId)
    {
        _imageInfoRepository.DeleteProfileImageInfoAsync(profileId);
    }

    private bool CheckIfProfileImageExists(string imageFileName, out string filePath)
    {
        filePath = BuildFilePath(_storageOptions.ImageDirectoryName, imageFileName);
        return File.Exists(filePath);
    }
}