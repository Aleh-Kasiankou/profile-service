using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Dto.MappingExtensions;
using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.AddressVerificationService.Implementations;

public class DummyAddressVerificationService : IAddressVerificationService
{
    public ProfileAddress VerifyAddress(ProfileAddressCreateUpdateDto address)
    {
        return address.ToProfileAddress();
    }

    public ProfileAddress VerifyAddress(ProfileAddressCreateUpdateDto address, ProfileAddress savedProfileAddress)
    {
        return VerifyAddress(address);
    }
}