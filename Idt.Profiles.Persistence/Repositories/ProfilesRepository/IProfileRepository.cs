using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Persistence.Repositories.ProfilesRepository;

public interface IProfileRepository
{
    Task<Profile> GetProfileAsync(Guid profileId);
    Task CreateProfileAsync(Profile profile);
    Task UpdateProfileAsync(Profile modifiedProfile, Profile savedProfile, bool isRollback = false);
    Task DeleteProfileAsync(Guid profileId);
}