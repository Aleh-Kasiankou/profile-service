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
            UserName = profile.LastName,
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
    
    public static ProfileAddress ToProfileAddress(this ProfileAddressCreateUpdateDto addressDto)
    {
        // TODO INJECT ADDRESS FORMATTER
        (string AddressLine1, string AddressLine2) FormatAddress()
        {
            var addressLine1 = $"{addressDto.Building} {addressDto.City}";
            var addressLine2 = addressDto.Apartment is null ? "" : $"apt. {addressDto.Apartment}";
            return (addressLine1, addressLine2);
        }

        var addressLines = FormatAddress();
        return new ProfileAddress
        {
            Apartment = addressDto.Apartment,
            Building = addressDto.Building,
            Street = addressDto.Street,
            City = addressDto.City,
            State = addressDto.State,
            ZipCode = addressDto.ZipCode,
            CountryCode = addressDto.CountryCode,
            AddressLine1 = addressLines.AddressLine1,
            AddressLine2 = addressLines.AddressLine2
        };
    }
}