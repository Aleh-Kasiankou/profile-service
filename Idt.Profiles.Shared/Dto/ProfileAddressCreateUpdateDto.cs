namespace Idt.Profiles.Shared.Dto;

public class ProfileAddressCreateUpdateDto
{
    public int Building { get; set; }
    public int? Apartment { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
}