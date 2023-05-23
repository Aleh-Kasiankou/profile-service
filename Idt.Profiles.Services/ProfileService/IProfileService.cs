using Idt.Profiles.Shared.Dto;

namespace Idt.Profiles.Services.ProfileService;

public interface IProfileService
{
    Task<ProfileDisplayDto> GetProfileAsync(Guid profileId);
    Task<ProfileDisplayDto> CreateProfileAsync(ProfileCreateUpdateDto profile);
    Task<ProfileDisplayDto> UpdateProfileAsync(Guid profileId, ProfileCreateUpdateDto profile);
    Task DeleteProfileAsync(Guid profileId);
}