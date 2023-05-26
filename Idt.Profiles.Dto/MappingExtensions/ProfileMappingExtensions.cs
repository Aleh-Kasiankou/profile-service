using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Dto.MappingExtensions;

public static class ProfileMappingExtensions
{
    public static ProfileDisplayDto ToDisplayDto(this Profile profile, string? profileImageUrl)
    {
        return new ProfileDisplayDto
        {
            ProfileId = profile.ProfileId,
            UserName = profile.UserName,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email,
            ProfileAddress = profile.ProfileAddress,
            ProfileImageUrl = profileImageUrl
        };
    }

    public static Profile ToProfileModel(this ProfileCreateUpdateDto profile, ProfileAddress address, Guid? profileId)
    {
        return new Profile
        {
            ProfileId = profileId ?? Guid.Empty,
            UserName = profile.UserName,
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email,
            ProfileAddress = address
        };
    }
    
}