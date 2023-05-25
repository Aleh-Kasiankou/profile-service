using System.ComponentModel.DataAnnotations;

namespace Idt.Profiles.Dto.Dto;

public class ProfileCreateUpdateDto
{
    [Required] public string UserName { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public ProfileAddressCreateUpdateDto ProfileAddress { get; set; }
}