using System.ComponentModel.DataAnnotations;

namespace Idt.Profiles.Dto.Dto;

public class ProfileAddressCreateUpdateDto
{
    public int? Apartment { get; set; }
    [Required] public int Building { get; set; }
    [Required] public string Street { get; set; }
    [Required] public string City { get; set; }
    [Required] public string State { get; set; }
    [Required] public string ZipCode { get; set; }
    [Required] public string CountryCode { get; set; }
}