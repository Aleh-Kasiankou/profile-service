using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Dto.MappingExtensions;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository;
using Idt.Profiles.Services.AddressVerificationService;
using Idt.Profiles.Services.ProfileImageService;
using Idt.Profiles.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Idt.Profiles.Services.ProfileService.Implementations;

public class ProfileService : IProfileService
{
    private readonly IAddressVerificationService _addressService;
    private readonly IProfileRepository _profileRepository;
    private readonly IProfileImageService _profileImageService;

    public ProfileService(IProfileRepository profileRepository, IProfileImageService profileImageService,
        IAddressVerificationService addressService)
    {
        _profileRepository = profileRepository;
        _profileImageService = profileImageService;
        _addressService = addressService;
    }

    public async Task<Profile> GetProfileAsync(Guid profileId)
    {
        return await _profileRepository.GetProfileAsync(profileId);
    }

    public async Task<Profile> CreateProfileAsync(ProfileCreateUpdateDto profile)
    {
        try
        {
            var address = _addressService.VerifyAddress(profile.ProfileAddress);
            var profileModel = profile.ToProfileModel(address, null);
            await _profileRepository.CreateProfileAsync(profileModel);
            return profileModel;
        }
        catch (MongoWriteException e)
        {
            if (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new ProfileWithSameUsernameAlreadyRegisteredException(profile.UserName);
            }

            throw;
        }
    }

    public async Task<Profile> UpdateProfileInfoAsync(Guid profileId, ProfileCreateUpdateDto profile)
    {
        var savedProfile = await GetProfileAsync(profileId);
        var address = _addressService.VerifyAddress(profile.ProfileAddress);
        var modifiedProfile = profile.ToProfileModel(address, profileId);
        return await UpdateProfileAsync(modifiedProfile, savedProfile);
    }

    public async Task UpdateProfileImageAsync(Guid profileId, IFormFile image)
    {
        await _profileImageService.UpdateProfileImageAsync(profileId, image);
    }

    public async Task DeleteProfileAsync(Guid profileId)
    {
        await _profileRepository.DeleteProfileAsync(profileId);
        _profileImageService.DeleteProfileImage(profileId);
    }

    private async Task<Profile> UpdateProfileAsync(Profile modifiedProfile, Profile savedProfile)
    {
        await _profileRepository.UpdateProfileAsync(modifiedProfile, savedProfile);
        return modifiedProfile;
    }
}