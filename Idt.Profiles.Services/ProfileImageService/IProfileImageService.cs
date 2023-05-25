using Idt.Profiles.Persistence.Models;
using Microsoft.AspNetCore.Http;

namespace Idt.Profiles.Services.ProfileImageService;

public interface IProfileImageService
{
    Task<(MemoryStream FileContent, string FileType)> GetProfileImageAsync(Guid profileId);
    Task UpdateProfileImageAsync(Guid profileId, IFormFile image);
    void DeleteProfileImage(Guid profileId);
}