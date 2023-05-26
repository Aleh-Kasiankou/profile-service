using Idt.Profiles.Dto.Dto;
using Swashbuckle.AspNetCore.Filters;

namespace Idt.Profiles.Api.SwaggerExamples;

public class ProfileExample : IExamplesProvider<ProfileCreateUpdateDto>
{
    public ProfileCreateUpdateDto GetExamples()
    {
        return new ProfileCreateUpdateDto
        {
            UserName = "funkyJazzer26",
            FirstName = "John",
            LastName = "Kravitz",
            Email = "john.kravitz@sample.com",
            ProfileAddress = new ProfileAddressCreateUpdateDto
            {
                Apartment = 26,
                Building = 5,
                Street = "Florida Ave.",
                City = "Austin",
                State = "Texas",
                ZipCode = "78617",
                CountryCode = "US"
            }
        };
    }
}