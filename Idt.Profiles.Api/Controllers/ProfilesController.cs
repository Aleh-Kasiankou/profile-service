using Idt.Profiles.Api.Controllers.BaseControllers;
using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Dto.MappingExtensions;
using Idt.Profiles.Services.ProfileImageService;
using Idt.Profiles.Services.ProfileService;
using Idt.Profiles.Shared.Exceptions.SystemCriticalExceptions;
using Microsoft.AspNetCore.Mvc;

namespace Idt.Profiles.Api.Controllers;

/// <summary>
/// Manages CRUD (Create, Read, Update, Delete) operations for user profiles and profile images.
/// </summary>
public class ProfilesController : BaseController
{
    private readonly IProfileService _profileService;
    private readonly IProfileImageService _profileImageService;

    private string BuildProfileImageEndpoint(Guid profileId) =>
        Url.Action("DownloadProfileImage", "Profiles", values: new { profileId }, protocol: Request.Scheme) ??
        throw new EndpointNotFoundException(nameof(DownloadProfileImageAsync));

    /// <inheritdoc />
    public ProfilesController(IProfileService profileService, IProfileImageService profileImageService)
    {
        _profileService = profileService;
        _profileImageService = profileImageService;
    }

    /// <summary>
    /// Returns profile information by profile id.
    /// </summary>
    /// <param name="profileId">Represents unique profile identifier.</param>
    /// <returns>Profile representation with user personal information (Name, Address, Link to profile image)</returns>
    /// <response code="200">Success. The user profile is located and returned.</response>
    [HttpGet("{profileId:guid}")]
    [ProducesResponseType(typeof(ProfileDisplayDto), 200)]
    public async Task<ActionResult<ProfileDisplayDto>> ViewProfile([FromRoute] Guid profileId)
    {
        var profile = await _profileService.GetProfileAsync(profileId);
        return Ok(profile.ToDisplayDto(BuildProfileImageEndpoint(profileId)));
    }

    /// <summary>
    /// Returns profile image by profile id. If no image is associated with the profile, default image is returned. 
    /// </summary>
    /// <param name="profileId">Represents unique profile identifier.</param>
    /// <returns>Image associated with user profile.</returns>
    /// <response code="200">Success. The user profile image is located and returned.</response>
    [HttpGet("{profileId:guid}/image")]
    [ProducesResponseType(typeof(FileResult), 200)]
    public async Task<IActionResult> DownloadProfileImageAsync([FromRoute] Guid profileId)
    {
        var imageFile = await _profileImageService.GetProfileImageAsync(profileId);
        return File(imageFile.FileContent, imageFile.FileType);
    }

    /// <summary>
    /// Creates profile with provided personal information.
    /// </summary>
    /// <param name="profile">Object with user personal information</param>
    /// <returns>Created user profile</returns>
    /// <response code="201">Success. The user profile is created and returned.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProfileDisplayDto), 201)]
    public async Task<ActionResult<ProfileDisplayDto>> CreateProfile([FromBody] ProfileCreateUpdateDto profile)
    {
        var savedProfile = await _profileService.CreateProfileAsync(profile);
        var savedProfileUri = Url.Action("ViewProfile", "Profiles", new { profileId = savedProfile.ProfileId }) ??
                              throw new EndpointNotFoundException(nameof(ViewProfile));
        return Created(savedProfileUri, savedProfile.ToDisplayDto(BuildProfileImageEndpoint(savedProfile.ProfileId)));
    }

    /// <summary>
    /// Uploads profile image to the server and associates it with user profile.
    /// </summary>
    /// <param name="profileId">Represents unique profile identifier.</param>
    /// <param name="image">Image file that should be associated with user profile.</param>
    /// <returns>Empty response with status code indicating operation success.</returns>
    /// <response code="200">Success. The user profile image is saved and associated with user profile.</response>
    [HttpPost("{profileId:guid}/image")]
    [ProducesResponseType(typeof(void), 200)]
    public async Task<IActionResult> UploadNewProfileImage([FromRoute] Guid profileId, IFormFile image)
    {
        await _profileService.UpdateProfileImageAsync(profileId, image);
        return Ok();
    }


    /// <summary>
    /// Updated personal information associated with user profile.
    /// </summary>
    /// <param name="profileId">Represents unique profile identifier.</param>
    /// <param name="profile">Object with updated user personal information</param>
    /// <returns>Representation of the updated user profile.</returns>
    /// <response code="200">Success. The user profile is updated and returned.</response>
    [HttpPut("{profileId:guid}")]
    [ProducesResponseType(typeof(ProfileDisplayDto), 200)]
    public async Task<ActionResult<ProfileDisplayDto>> UpdateProfile([FromRoute] Guid profileId,
        [FromBody] ProfileCreateUpdateDto profile)
    {
        var updatedProfile = await _profileService.UpdateProfileInfoAsync(profileId, profile);
        return Ok(updatedProfile.ToDisplayDto(BuildProfileImageEndpoint(profileId)));
    }

    /// <summary>
    /// Deletes user profile with the provided id.
    /// </summary>
    /// <param name="profileId">Represents unique profile identifier.</param>
    /// <returns>Empty response with status code indicating operation success.</returns>
    /// <response code="204">Success. The user profile is deleted.</response>
    [HttpDelete("{profileId:guid}")]
    [ProducesResponseType(typeof(void), 204)]
    public async Task<ActionResult> DeleteProfile([FromRoute] Guid profileId)
    {
        await _profileService.DeleteProfileAsync(profileId);
        return NoContent();
    }

    /// <summary>
    /// Deletes image associated with user profile if there is one.
    /// </summary>
    /// <param name="profileId">Represents unique profile identifier.</param>
    /// <returns>Empty response with status code indicating operation success.</returns>
    /// <response code="204">Success. The user profile image is deleted.</response>
    [HttpDelete("{profileId:guid}/image")]
    [ProducesResponseType(typeof(void), 204)]
    public IActionResult DeleteProfileImage([FromRoute] Guid profileId)
    {
        _profileImageService.DeleteProfileImage(profileId);
        return NoContent();
    }
}