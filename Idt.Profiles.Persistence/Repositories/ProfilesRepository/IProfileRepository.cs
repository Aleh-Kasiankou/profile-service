using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Persistence.Repositories.ProfilesRepository;

public interface IProfileRepository
{
    Task<Profile> GetProfileAsync(Guid profileId);
    Task CreateProfileAsync(Profile profile);
    Task UpdateProfileAsync(Profile profile);
    Task DeleteProfileAsync(Guid profileId);
}