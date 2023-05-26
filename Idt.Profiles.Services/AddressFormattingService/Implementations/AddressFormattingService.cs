using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.AddressFormattingService.Implementations;

public class AddressFormattingService : IAddressFormattingService
{
    public ProfileAddress FormatAddress(ProfileAddressCreateUpdateDto address)
    {
        var addressLines = GenerateAddressLines(address);
        return new ProfileAddress
        {
            Apartment = address.Apartment,
            Building = address.Building,
            Street = address.Street,
            City = address.City,
            State = address.State,
            ZipCode = address.ZipCode,
            CountryCode = address.CountryCode,
            AddressLine1 = addressLines.AddressLine1,
            AddressLine2 = addressLines.AddressLine2
        };
    }

    private (string AddressLine1, string AddressLine2) GenerateAddressLines(ProfileAddressCreateUpdateDto address)
    {
        var addressLine1 = $"{address.Building} {address.Street}";
        var addressLine2 = address.Apartment is null ? "" : $"apt. {address.Apartment}";
        return (addressLine1, addressLine2);
    }
}