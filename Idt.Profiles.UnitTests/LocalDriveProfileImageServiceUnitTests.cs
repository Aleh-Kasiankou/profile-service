using FluentValidation;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Persistence.Repositories.ProfileImageRepository;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository;
using Idt.Profiles.Services.FileManagementService;
using Idt.Profiles.Services.ProfileImageService.Implementations;
using Idt.Profiles.Shared.ConfigurationOptions;
using Idt.Profiles.Shared.Exceptions.ClientRelatedExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Guid = System.Guid;

namespace Idt.Profiles.UnitTests;

public class LocalDriveProfileImageServiceUnitTests
{
    [Fact]
    public async Task GetProfileImageAsync_IfProfileExistsAndHasAssociatedImage_ReturnsAssociatedImage()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var imageInfoRepositoryMock =
            LocalDriveProfileImageServiceTestHelper.MockProfileImageInfoRepository();
        var imageValidatorMock = LocalDriveProfileImageServiceTestHelper.MockImageValidator();
        var profileRepositoryMock = LocalDriveProfileImageServiceTestHelper.MockProfileRepository();
        var fileManagerMock = LocalDriveProfileImageServiceTestHelper.MockFileManagerService();

        var profileImageService =
            LocalDriveProfileImageServiceTestHelper.BuildTestingProfileImageService(imageInfoRepositoryMock,
                imageValidatorMock,
                profileRepositoryMock, fileManagerMock);

        var fileInfo = LocalDriveProfileImageServiceTestHelper.CreateFakeIFormFile();
        await profileRepositoryMock.CreateProfileAsync(new Profile { ProfileId = profileId });
        await profileImageService.UpdateProfileImageAsync(profileId, fileInfo.file);

        // Act
        var savedImage = await profileImageService.GetProfileImageAsync(profileId);

        // Assert
        Assert.Equal(fileInfo.file.ContentType, savedImage.FileType);
        Assert.Equal(fileInfo.content.Length, savedImage.FileContent.Length);
    }

    [Fact]
    public async Task GetProfileImageAsync_IfProfileExistsAndDoesNotHaveAssociatedImage_ReturnsDefaultImage()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var imageInfoRepositoryMock =
            LocalDriveProfileImageServiceTestHelper.MockProfileImageInfoRepository();
        var imageValidatorMock = LocalDriveProfileImageServiceTestHelper.MockImageValidator();
        var profileRepositoryMock = LocalDriveProfileImageServiceTestHelper.MockProfileRepository();
        var fileManagerMock = LocalDriveProfileImageServiceTestHelper.MockFileManagerService();

        var profileImageService =
            LocalDriveProfileImageServiceTestHelper.BuildTestingProfileImageService(imageInfoRepositoryMock,
                imageValidatorMock,
                profileRepositoryMock, fileManagerMock);
        // Act

        var savedImage = await profileImageService.GetProfileImageAsync(profileId);

        // Assert
        Assert.Equal(LocalDriveProfileImageServiceTestHelper.DefaultProfileImageType, savedImage.FileType);
        Assert.Equal(LocalDriveProfileImageServiceTestHelper.DefaultFileContent, savedImage.FileContent);
    }

    [Fact]
    public async Task UpdateProfileImageAsync_IfProfileIsFoundAndImageIsValid_SavesImage()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var imageInfoRepositoryMock =
            LocalDriveProfileImageServiceTestHelper.MockProfileImageInfoRepository();
        var imageValidatorMock = LocalDriveProfileImageServiceTestHelper.MockImageValidator();
        var profileRepositoryMock = LocalDriveProfileImageServiceTestHelper.MockProfileRepository();
        var fileManagerMock = LocalDriveProfileImageServiceTestHelper.MockFileManagerService();

        var profileImageService =
            LocalDriveProfileImageServiceTestHelper.BuildTestingProfileImageService(imageInfoRepositoryMock,
                imageValidatorMock,
                profileRepositoryMock, fileManagerMock);

        var fileInfo = LocalDriveProfileImageServiceTestHelper.CreateFakeIFormFile();
        await profileRepositoryMock.CreateProfileAsync(new Profile { ProfileId = profileId });

        // Act

        await profileImageService.UpdateProfileImageAsync(profileId, fileInfo.file);

        // Assert
        Assert.NotNull(imageInfoRepositoryMock.GetProfileImageInfoAsync(profileId));
    }

    [Fact]
    public async Task UpdateProfileImageAsync_IfProfileIsNotFound_DoesNotSaveImage()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var imageInfoRepositoryMock =
            LocalDriveProfileImageServiceTestHelper.MockProfileImageInfoRepository();
        var imageValidatorMock = LocalDriveProfileImageServiceTestHelper.MockImageValidator();
        var profileRepositoryMock = LocalDriveProfileImageServiceTestHelper.MockProfileRepository();
        var fileManagerMock = LocalDriveProfileImageServiceTestHelper.MockFileManagerService();

        var profileImageService =
            LocalDriveProfileImageServiceTestHelper.BuildTestingProfileImageService(imageInfoRepositoryMock,
                imageValidatorMock,
                profileRepositoryMock, fileManagerMock);
        var fileInfo = LocalDriveProfileImageServiceTestHelper.CreateFakeIFormFile();


        // Act && Assert
        await Assert.ThrowsAsync<ProfileDoesNotExistException>(async () =>
            await profileImageService.UpdateProfileImageAsync(profileId, fileInfo.file));
    }

    [Fact]
    public async Task UpdateProfileImageAsync_IfFileIsInvalid_DoesNotSaveImage()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var imageInfoRepositoryMock =
            LocalDriveProfileImageServiceTestHelper.MockProfileImageInfoRepository();
        var imageValidatorMock = LocalDriveProfileImageServiceTestHelper.MockImageValidator(true);
        var profileRepositoryMock = LocalDriveProfileImageServiceTestHelper.MockProfileRepository();
        var fileManagerMock = LocalDriveProfileImageServiceTestHelper.MockFileManagerService();

        var profileImageService =
            LocalDriveProfileImageServiceTestHelper.BuildTestingProfileImageService(imageInfoRepositoryMock,
                imageValidatorMock,
                profileRepositoryMock, fileManagerMock);
        var fileInfo = LocalDriveProfileImageServiceTestHelper.CreateFakeIFormFile();
        await profileRepositoryMock.CreateProfileAsync(new Profile { ProfileId = profileId });

        // Act && Assert
        await Assert.ThrowsAsync<ValidationException>(async () =>
            await profileImageService.UpdateProfileImageAsync(profileId, fileInfo.file));
    }

    [Fact]
    public async Task DeleteProfileImage_IfFileExists_DeletesImage()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var imageInfoRepositoryMock =
            LocalDriveProfileImageServiceTestHelper.MockProfileImageInfoRepository();
        var imageValidatorMock = LocalDriveProfileImageServiceTestHelper.MockImageValidator();
        var profileRepositoryMock = LocalDriveProfileImageServiceTestHelper.MockProfileRepository();
        var fileManagerMock = LocalDriveProfileImageServiceTestHelper.MockFileManagerService();

        var profileImageService =
            LocalDriveProfileImageServiceTestHelper.BuildTestingProfileImageService(imageInfoRepositoryMock,
                imageValidatorMock,
                profileRepositoryMock, fileManagerMock);

        var fileInfo = LocalDriveProfileImageServiceTestHelper.CreateFakeIFormFile();
        await profileRepositoryMock.CreateProfileAsync(new Profile { ProfileId = profileId });
        await profileImageService.UpdateProfileImageAsync(profileId, fileInfo.file);

        // Act
        profileImageService.DeleteProfileImage(profileId);

        // Assert
        Assert.Null(await imageInfoRepositoryMock.GetProfileImageInfoAsync(profileId));
    }

    [Fact]
    public async Task DeleteProfileImage_IfFileDoesNotExist_DoesNotThrowException()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var imageInfoRepositoryMock =
            LocalDriveProfileImageServiceTestHelper.MockProfileImageInfoRepository();
        var imageValidatorMock = LocalDriveProfileImageServiceTestHelper.MockImageValidator();
        var profileRepositoryMock = LocalDriveProfileImageServiceTestHelper.MockProfileRepository();
        var fileManagerMock = LocalDriveProfileImageServiceTestHelper.MockFileManagerService();

        var profileImageService =
            LocalDriveProfileImageServiceTestHelper.BuildTestingProfileImageService(imageInfoRepositoryMock,
                imageValidatorMock,
                profileRepositoryMock, fileManagerMock);


        // Act
        profileImageService.DeleteProfileImage(profileId);

        // Assert
        Assert.Null(await imageInfoRepositoryMock.GetProfileImageInfoAsync(profileId));
    }

    static class LocalDriveProfileImageServiceTestHelper
    {
        public const string ImageDirectoryName = "TestData";
        public const string DefaultProfileImageName = "blank-profile-picture.webp";
        public const string DefaultProfileImageType = "image/webp";

        public static readonly MemoryStream
            DefaultFileContent = new("fileContenT"u8.ToArray());


        public static LocalDriveProfileImageService BuildTestingProfileImageService(
            IProfileImageInfoRepository infoRepositoryMock, IValidator<IFormFile> validatorMock,
            IProfileRepository profileRepositoryMock, IFileManagementService fileManagementServiceMock)
        {
            var imageStorageOptions = Options.Create(new LocalDriveImageStorageOptions
            {
                ImageDirectoryName = ImageDirectoryName,
                DefaultProfileImageName = DefaultProfileImageName,
                DefaultProfileImageType = DefaultProfileImageType
            });

            var profileImageService =
                new LocalDriveProfileImageService(imageStorageOptions, infoRepositoryMock, validatorMock,
                    profileRepositoryMock, fileManagementServiceMock);

            return profileImageService;
        }

        public static ProfileImageInfo CreateProfileImageInfo(Guid profileId)
        {
            return new ProfileImageInfo
            {
                ProfileId = profileId,
                FileName = profileId.ToString(),
                FileType = DefaultProfileImageType
            };
        }

        public static IProfileImageInfoRepository MockProfileImageInfoRepository()
        {
            var databaseSimulation = new Dictionary<Guid, ProfileImageInfo>();

            Task WriteToRepository(ProfileImageInfo imageInfo)
            {
                databaseSimulation.Add(imageInfo.ProfileId, imageInfo);
                return Task.CompletedTask;
            }

            Task<ProfileImageInfo?> GetFromRepository(Guid profileId)
            {
                databaseSimulation.TryGetValue(profileId, out var value);
                return Task.FromResult(value);
            }

            Task DeleteFromRepository(Guid profileId)
            {
                databaseSimulation.TryGetValue(profileId, out var value);
                if (value != null)
                {
                    databaseSimulation.Remove(profileId);
                }

                return Task.CompletedTask;
            }


            var imageInfoRepositoryMockBuilder = new Mock<IProfileImageInfoRepository>();
            imageInfoRepositoryMockBuilder.Setup(x => x.SaveProfileImageInfoAsync(It.IsAny<ProfileImageInfo>()))
                .Returns((ProfileImageInfo x) => WriteToRepository(x));

            imageInfoRepositoryMockBuilder.Setup(x => x.GetProfileImageInfoAsync(It.IsAny<Guid>()))
                .Returns((Guid id) => GetFromRepository(id));

            imageInfoRepositoryMockBuilder.Setup(x => x.DeleteProfileImageInfoAsync(It.IsAny<Guid>()))
                .Returns((Guid profileId) => DeleteFromRepository(profileId));

            return imageInfoRepositoryMockBuilder.Object;
        }

        public static IValidator<IFormFile> MockImageValidator(bool fails = false)
        {
            var imageValidatorMock = new InlineValidator<IFormFile>
            {
                x => x.RuleFor(im => im).Must(im => !fails)
            };
            return imageValidatorMock;
        }

        public static IProfileRepository MockProfileRepository()
        {
            var localStorageSimulation = new Dictionary<Guid, Profile>();

            Task AddProfile(Profile profile)
            {
                localStorageSimulation.Add(profile.ProfileId, profile);
                return Task.CompletedTask;
            }

            Task<Profile> GetProfile(Guid profileId)
            {
                localStorageSimulation.TryGetValue(profileId, out var profile);
                if (profile is null)
                {
                    throw new ProfileDoesNotExistException(profileId);
                }

                return Task.FromResult(profile);
            }

            var profileRepositoryMockBuilder = new Mock<IProfileRepository>();
            profileRepositoryMockBuilder.Setup(x => x.GetProfileAsync(It.IsAny<Guid>()))
                .Returns((Guid profileId) => GetProfile(profileId));
            profileRepositoryMockBuilder.Setup(x => x.CreateProfileAsync(It.IsAny<Profile>()))
                .Returns((Profile profile) => AddProfile(profile));


            return profileRepositoryMockBuilder.Object;
        }

        public static IFileManagementService MockFileManagerService()
        {
            var localStorageSimulation = new Dictionary<string, MemoryStream>
            {
                { Path.Combine(ImageDirectoryName, DefaultProfileImageName), DefaultFileContent }
            };

            string BuildFilePath(string directory, string fileName) => Path.Combine(directory, fileName);
            Task<MemoryStream> GetFileFromDrive(string filepath) => Task.FromResult(localStorageSimulation[filepath]);
            bool CheckIfFileExists(string filepath) => localStorageSimulation.ContainsKey(filepath);

            Task SaveFile(string filePath, IFormFile file)
            {
                var fileContent = new MemoryStream();
                file.CopyTo(fileContent);
                localStorageSimulation.Add(filePath, fileContent);
                return Task.CompletedTask;
            }


            var fileManagerMockBuilder = new Mock<IFileManagementService>();
            fileManagerMockBuilder.Setup(x => x.BuildFilePath(It.IsAny<string>()
                , It.IsAny<string>())).Returns<string, string>((x, t) => BuildFilePath(x, t));
            fileManagerMockBuilder.Setup(x => x.GetFileContent(It.IsAny<string>()))
                .Returns((string x) => GetFileFromDrive(x));
            fileManagerMockBuilder.Setup(x => x.CheckIfFileExists(It.IsAny<string>()))
                .Returns((string x) => CheckIfFileExists(x));
            fileManagerMockBuilder.Setup(x => x.WriteImageToLocalDriveAsync(It.IsAny<string>(), It.IsAny<IFormFile>()))
                .Returns((string x, IFormFile t) => SaveFile(x, t));
            return fileManagerMockBuilder.Object;
        }

        public static (IFormFile file, MemoryStream content) CreateFakeIFormFile()
        {
            var content = "Hello World from a Fake File";
            var fileName = "AnyName.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            var formFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName)
                { Headers = new HeaderDictionary() };

            return (formFile, stream);
        }
    }
}