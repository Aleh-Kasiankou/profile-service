using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Shared.Dto;

public class ProfileCreateUpdateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ProfileAddress ProfileAddress { get; set; }
}