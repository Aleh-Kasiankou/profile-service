namespace Idt.Profiles.Persistence.Models;

public class ProfileAddress
{
    public Guid ProfileAddressId { get; set; }
    public int Building { get; set; }
    public int? Apartment { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
}