using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Persistence.Repositories.ProfileImageRepository;

public interface IProfileImageInfoRepository
{
    Task<ProfileImageInfo?> GetProfileImageInfoAsync(Guid profileId);
    Task SaveProfileImageInfoAsync(ProfileImageInfo image);
    Task DeleteProfileImageInfoAsync(Guid profileId);
}