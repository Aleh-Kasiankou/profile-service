using FluentValidation;
using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Dto.MappingExtensions;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository;
using Idt.Profiles.Services.AddressFormattingService;
using Idt.Profiles.Services.ProfileImageService;
using Idt.Profiles.Shared.Exceptions.ClientRelatedExceptions;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Idt.Profiles.Services.ProfileService.Implementations;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAddressFormattingService _addressFormattingService;
    private readonly IProfileImageService _profileImageService;
    private readonly IValidator<Profile> _profileValidator;
    private readonly IValidator<ProfileAddressCreateUpdateDto> _addressValidator;

    public ProfileService(IValidator<Profile> profileValidator,
        IValidator<ProfileAddressCreateUpdateDto> addressValidator, IProfileRepository profileRepository,
        IAddressFormattingService addressFormattingService,
        IProfileImageService profileImageService)
    {
        _profileValidator = profileValidator;
        _addressValidator = addressValidator;
        _profileRepository = profileRepository;
        _addressFormattingService = addressFormattingService;
        _profileImageService = profileImageService;
    }

    public async Task<Profile> GetProfileAsync(Guid profileId)
    {
        return await _profileRepository.GetProfileAsync(profileId);
    }

    public async Task<Profile> CreateProfileAsync(ProfileCreateUpdateDto profile)
    {
        try
        {
            await _addressValidator.ValidateAndThrowAsync(profile.ProfileAddress);
            var address = _addressFormattingService.FormatAddress(profile.ProfileAddress);
            var profileModel = profile.ToProfileModel(address, null);
            _profileValidator.ValidateAndThrow(profileModel);
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
        await _addressValidator.ValidateAndThrowAsync(profile.ProfileAddress);
        var address = _addressFormattingService.FormatAddress(profile.ProfileAddress);
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
        _profileValidator.ValidateAndThrow(modifiedProfile);
        await _profileRepository.UpdateProfileAsync(modifiedProfile, savedProfile);
        return modifiedProfile;
    }
}