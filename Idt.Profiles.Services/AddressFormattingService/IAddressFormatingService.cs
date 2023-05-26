using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Persistence.Models;

namespace Idt.Profiles.Services.AddressFormattingService;

public interface IAddressFormattingService
{
    ProfileAddress FormatAddress(ProfileAddressCreateUpdateDto address);
}