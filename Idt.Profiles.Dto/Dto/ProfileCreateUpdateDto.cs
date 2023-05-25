using System.ComponentModel.DataAnnotations;

namespace Idt.Profiles.Dto.Dto;

public class ProfileCreateUpdateDto
{
    /// <summary>
    /// Username / Nickname.
    /// </summary>
    [Required] public string UserName { get; set; }
    /// <summary>
    /// User first name.
    /// </summary>
    [Required] public string FirstName { get; set; }
    /// <summary>
    /// User last name.
    /// </summary>
    [Required] public string LastName { get; set; }
    /// <summary>
    /// User email address.
    /// </summary>
    [Required] [EmailAddress] public string Email { get; set; }
    /// <summary>
    /// User personal address
    /// </summary>
    [Required] public ProfileAddressCreateUpdateDto ProfileAddress { get; set; }
}