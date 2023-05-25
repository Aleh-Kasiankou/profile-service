using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.AddressVerificationService;

public interface IAddressVerificationService
{
    ProfileAddress VerifyAddress(ProfileAddressCreateUpdateDto address);
    ProfileAddress VerifyAddress(ProfileAddressCreateUpdateDto address, ProfileAddress savedProfileAddress);

}