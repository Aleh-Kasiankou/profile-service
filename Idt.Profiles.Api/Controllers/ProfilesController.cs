using Idt.Profiles.Api.Controllers.BaseControllers;
using Idt.Profiles.Services.ProfileService;
using Idt.Profiles.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Idt.Profiles.Api.Controllers;

public class ProfilesController : BaseController
{
    private readonly ILogger<ProfilesController> _logger;
    private readonly IProfileService _profileService;

    public ProfilesController(IProfileService profileService, ILogger<ProfilesController> logger)
    {
        _profileService = profileService;
        _logger = logger;
    }

    [HttpGet("{profileId:guid}")]
    public ActionResult<ProfileDisplayDto> ViewProfile([FromRoute] Guid profileId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public ActionResult<ProfileDisplayDto> CreateProfile([FromBody] ProfileCreateUpdateDto profile)
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("{profileId:guid}")]
    public ActionResult<ProfileDisplayDto> UpdateProfile([FromRoute] Guid profileId ,[FromBody] ProfileCreateUpdateDto profile)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{profileId:guid}")]
    public ActionResult<ProfileDisplayDto> DeleteProfile([FromRoute] Guid profileId)
    {
        throw new NotImplementedException();
    }
}