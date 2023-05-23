using Idt.Profiles.Api.Controllers.BaseControllers;
using Idt.Profiles.Services.ProfileImageService;
using Microsoft.AspNetCore.Mvc;

namespace Idt.Profiles.Api.Controllers;

public class ProfileImagesController : BaseController
{
    private readonly ILogger<ProfileImagesController> _logger;
    private readonly IProfileImageService _profileImageService;

    public ProfileImagesController(IProfileImageService profileImageService, ILogger<ProfileImagesController> logger)
    {
        _profileImageService = profileImageService;
        _logger = logger;
    }

    [HttpPost("{profileId:guid}")]
    public async Task<IActionResult> UploadNewProfileImage([FromRoute] Guid profileId, [FromForm] IFormFile image)
    {
        await _profileImageService.UpdateProfileImageAsync(profileId, image);
        return Ok();
    }
    
    [HttpDelete("{profileId:guid}")]
    public async Task<IActionResult> DeleteProfileImage([FromRoute] Guid profileId)
    {
        await _profileImageService.DeleteProfileImageAsync(profileId);
        return NoContent();
    }
}