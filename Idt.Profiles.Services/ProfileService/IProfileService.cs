using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Persistence.Models;
using Microsoft.AspNetCore.Http;

namespace Idt.Profiles.Services.ProfileService;

public interface IProfileService
{
    Task<Profile> GetProfileAsync(Guid profileId);
    Task<Profile> CreateProfileAsync(ProfileCreateUpdateDto profile);
    Task<Profile> UpdateProfileInfoAsync(Guid profileId, ProfileCreateUpdateDto profile);
    Task UpdateProfileImageAsync(Guid profileId, IFormFile image);
    Task DeleteProfileAsync(Guid profileId);
}