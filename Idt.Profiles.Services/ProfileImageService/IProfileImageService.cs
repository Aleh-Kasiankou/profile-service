using Microsoft.AspNetCore.Http;

namespace Idt.Profiles.Services.ProfileImageService;

public interface IProfileImageService
{
    Task<string?> GetProfileImageUrlAsync(Guid profileId);
    Task UpdateProfileImageAsync(Guid profileId, IFormFile image);
    Task DeleteProfileImageAsync(Guid profileId);
}