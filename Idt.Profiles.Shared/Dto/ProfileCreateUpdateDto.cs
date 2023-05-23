namespace Idt.Profiles.Shared.Dto;

public class ProfileCreateUpdateDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ProfileAddressCreateUpdateDto ProfileAddress { get; set; }
}