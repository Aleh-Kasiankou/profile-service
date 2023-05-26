using System.ComponentModel.DataAnnotations;

namespace Idt.Profiles.Dto.Dto;

/// <summary>
/// Object with user personal address.
/// </summary>
public class ProfileAddressCreateUpdateDto
{
    /// <summary>
    /// Apartment number.
    /// </summary>
    public int? Apartment { get; set; }
    /// <summary>
    /// Building number.
    /// </summary>
    [Required] public int Building { get; set; }
    /// <summary>
    /// Street.
    /// </summary>
    [Required] public string Street { get; set; }
    /// <summary>
    /// City/Town/Locality
    /// </summary>
    [Required] public string City { get; set; }
    /// <summary>
    /// State/Region
    /// </summary>
    [Required] public string State { get; set; }
    /// <summary>
    /// Zip code / Postal code
    /// </summary>
    [Required] public string ZipCode { get; set; }
    /// <summary>
    /// Country code (ISO 3166 Alpha-2)
    /// </summary>
    [Required] public string CountryCode { get; set; }
}