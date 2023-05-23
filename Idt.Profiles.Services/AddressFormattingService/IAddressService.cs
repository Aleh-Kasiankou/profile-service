using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Shared.Dto;

namespace Idt.Profiles.Services.AddressFormattingService;

public interface IAddressService
{
    ProfileAddress VerifyAddress(ProfileAddressCreateUpdateDto address);
}