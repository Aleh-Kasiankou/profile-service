namespace Idt.Profiles.Persistence.Models;

public class Profile
{
    public Guid ProfileId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ProfileAddress ProfileAddress { get; set; }
    public ProfilePicture? ProfilePicture { get; set; }
}