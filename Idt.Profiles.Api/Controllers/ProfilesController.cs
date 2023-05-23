using Idt.Profiles.Api.Controllers.BaseControllers;
using Idt.Profiles.Services.ProfileService;
using Idt.Profiles.Shared.Dto;
using Idt.Profiles.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Idt.Profiles.Api.Controllers;

public class ProfilesController : BaseController
{
    private readonly IProfileService _profileService;

    public ProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("{profileId:guid}")]
    public async Task<ActionResult<ProfileDisplayDto>> ViewProfile([FromRoute] Guid profileId)
    {
        return Ok(await _profileService.GetProfileAsync(profileId));
    }

    [HttpPost]
    public async Task<ActionResult<ProfileDisplayDto>> CreateProfile([FromBody] ProfileCreateUpdateDto profile)
    {
        var savedProfile = await _profileService.CreateProfileAsync(profile);
        var savedProfileUri = Url.Action(nameof(ViewProfile), savedProfile.ProfileId) ??
                              throw new EndpointNotFoundException(nameof(ViewProfile));
        return Created(savedProfileUri, savedProfile);
    }

    [HttpPut("{profileId:guid}")]
    public async Task<ActionResult<ProfileDisplayDto>> UpdateProfile([FromRoute] Guid profileId,
        [FromBody] ProfileCreateUpdateDto profile)
    {
        var updatedProfile = await _profileService.UpdateProfileAsync(profileId, profile);
        return Ok(updatedProfile);
    }

    [HttpDelete("{profileId:guid}")]
    public async Task<ActionResult> DeleteProfile([FromRoute] Guid profileId)
    {
        await _profileService.DeleteProfileAsync(profileId);
        return NoContent();
    }
}