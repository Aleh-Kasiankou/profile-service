using Idt.Profiles.Api.Controllers.BaseControllers;
using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Dto.MappingExtensions;
using Idt.Profiles.Services.ProfileImageService;
using Idt.Profiles.Services.ProfileService;
using Idt.Profiles.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Idt.Profiles.Api.Controllers;

public class ProfilesController : BaseController
{
    private readonly IProfileService _profileService;
    private readonly IProfileImageService _profileImageService;

    private string BuildProfileImageEndpoint(Guid profileId) => Url.Action(nameof(DownloadProfileImageAsync), "Profiles", values: new{profileId}, protocol: Request.Scheme) ??
                                                                throw new EndpointNotFoundException(
                                                                    nameof(DownloadProfileImageAsync));

    public ProfilesController(IProfileService profileService, IProfileImageService profileImageService)
    {
        _profileService = profileService;
        _profileImageService = profileImageService;
    }

    [HttpGet("{profileId:guid}")]
    public async Task<ActionResult<ProfileDisplayDto>> ViewProfile([FromRoute] Guid profileId)
    {
        var profile = await _profileService.GetProfileAsync(profileId);
        return Ok(profile.ToDisplayDto(BuildProfileImageEndpoint(profileId)));
    }

    [HttpGet("{profileId:guid}/image")]
    public async Task<IActionResult> DownloadProfileImageAsync([FromRoute] Guid profileId)
    {
        var imageFile = await _profileImageService.GetProfileImageAsync(profileId);
        return File(imageFile.FileContent, imageFile.FileType);
    }

    [HttpPost]
    public async Task<ActionResult<ProfileDisplayDto>> CreateProfile([FromBody] ProfileCreateUpdateDto profile)
    {
        var savedProfile = await _profileService.CreateProfileAsync(profile);
        var savedProfileUri = Url.Action("ViewProfile", "Profiles", new { profileId = savedProfile.ProfileId }) ??
                              throw new EndpointNotFoundException(nameof(ViewProfile));
        return Created(savedProfileUri, savedProfile.ToDisplayDto(BuildProfileImageEndpoint(savedProfile.ProfileId)));
    }

    [HttpPost("{profileId:guid}/image")]
    public async Task<IActionResult> UploadNewProfileImage([FromRoute] Guid profileId, IFormFile image)
    {
        await _profileService.UpdateProfileImageAsync(profileId, image);
        return Ok();
    }


    [HttpPut("{profileId:guid}")]
    public async Task<ActionResult<ProfileDisplayDto>> UpdateProfile([FromRoute] Guid profileId,
        [FromBody] ProfileCreateUpdateDto profile)
    {
        var updatedProfile = await _profileService.UpdateProfileInfoAsync(profileId, profile);
        return Ok(updatedProfile.ToDisplayDto(BuildProfileImageEndpoint(profileId)));
    }

    [HttpDelete("{profileId:guid}")]
    public async Task<ActionResult> DeleteProfile([FromRoute] Guid profileId)
    {
        await _profileService.DeleteProfileAsync(profileId);
        return NoContent();
    }

    [HttpDelete("{profileId:guid}/image")]
    public IActionResult DeleteProfileImage([FromRoute] Guid profileId)
    {
        _profileImageService.DeleteProfileImage(profileId);
        return NoContent();
    }
}