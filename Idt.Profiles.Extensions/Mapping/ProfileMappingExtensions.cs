using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Shared.Dto;

namespace Idt.Profiles.Extensions.Mapping;

public static class ProfileMappingExtensions
{
    public static ProfileDisplayDto ToDisplayDto(this Profile profile, string? profileImageUrl)
    {
        return new ProfileDisplayDto
        {
            ProfileId = profile.ProfileId,
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
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email,
            ProfileAddress = address,
        };
    }
}