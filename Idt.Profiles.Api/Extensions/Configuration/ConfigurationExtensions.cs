using Idt.Profiles.Persistence.Repositories.ProfileImageRepository;
using Idt.Profiles.Persistence.Repositories.ProfileImageRepository.Implementations;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository.Implementations;
using Idt.Profiles.Services.AddressVerificationService;
using Idt.Profiles.Services.AddressVerificationService.Implementations;
using Idt.Profiles.Services.ProfileImageService;
using Idt.Profiles.Services.ProfileImageService.Implementations;
using Idt.Profiles.Services.ProfileService;
using Idt.Profiles.Services.ProfileService.Implementations;
using Idt.Profiles.Shared.ConfigurationOptions;

namespace Idt.Profiles.Api.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbConfigurationOptions>(configuration.GetSection("MongoDb"));
        services.AddScoped<IProfileRepository, MongoTransactionalProfileRepository>();
        services.AddScoped<IProfileImageInfoRepository, ProfileImageInfoRepository>();
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LocalDriveImageStorageOptions>(configuration.GetSection("ImageStorage"));
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IProfileImageService, LocalDriveProfileImageService>();
        services.AddScoped<IAddressVerificationService, DummyAddressVerificationService>();
        return services;
    }
}