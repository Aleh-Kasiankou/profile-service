
using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Dto.Dto;

/// <summary>
/// Representation of user profile.
/// </summary>
public class ProfileDisplayDto
{
    /// <summary>
    /// Unique profile identifier.
    /// </summary>
    public Guid ProfileId { get; set; }
    /// <summary>
    /// Unique profile username/nickname.
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// First name of the profile owner.
    /// </summary>
    public string FirstName { get; set; }
    /// <summary>
    /// Last name of the profile owner.
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    /// Email address of the profile owner.
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// Personal address of the profile owner.
    /// </summary>
    public ProfileAddress ProfileAddress { get; set; }
    /// <summary>
    /// Url for downloading profile image.
    /// </summary>
    public string? ProfileImageUrl { get; set; }
}