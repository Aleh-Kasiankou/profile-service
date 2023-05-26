using FluentValidation;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Idt.Profiles.Api.Middleware.ExceptionHandling;
using Idt.Profiles.Api.SwaggerExamples;
using Idt.Profiles.Dto.Dto;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Persistence.Repositories.ProfileImageRepository;
using Idt.Profiles.Persistence.Repositories.ProfileImageRepository.Implementations;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository;
using Idt.Profiles.Persistence.Repositories.ProfilesRepository.Implementations;
using Idt.Profiles.Services.AddressFormattingService;
using Idt.Profiles.Services.AddressFormattingService.Implementations;
using Idt.Profiles.Services.EventSyncService;
using Idt.Profiles.Services.EventSyncService.Implementations;
using Idt.Profiles.Services.FileManagementService;
using Idt.Profiles.Services.FileManagementService.Implementations;
using Idt.Profiles.Services.FluentValidationService.Validators;
using Idt.Profiles.Services.ProfileImageService;
using Idt.Profiles.Services.ProfileImageService.Implementations;
using Idt.Profiles.Services.ProfileService;
using Idt.Profiles.Services.ProfileService.Implementations;
using Idt.Profiles.Shared.ConfigurationOptions;
using MongoDB.Driver;
using Moq;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

namespace Idt.Profiles.Api.Extensions.Configuration;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbConfigurationOptions>(configuration.GetSection("MongoDb"));
        services.AddSingleton<IMongoClient>(new MongoClient(configuration["MongoDb:ConnectionString"]));
        services.AddScoped<IProfileRepository, MongoTransactionalProfileRepository>();
        services.AddScoped<IProfileImageInfoRepository, ProfileImageInfoRepository>();
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LocalDriveImageStorageOptions>(configuration.GetSection("ImageStorage"));
        services.Configure<MongoRetryRollbackPolicyOptions>(configuration.GetSection("MongoRollbackRetryPolicy"));
        services.AddHangfire(x => x.UseMongoStorage(configuration["MongoDb:ConnectionString"],
            configuration["MongoDb:HangfireDatabase"], new MongoStorageOptions
            {
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection,
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                Prefix = "hangfire.mongo",
                CheckConnection = true
            }));
        services.AddHangfireServer();
        services.AddTransient<IEventSyncService, EventSyncService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IProfileImageService, LocalDriveProfileImageService>();
        services.AddScoped<IAddressFormattingService, AddressFormattingService>();
        services.AddSingleton<IFileManagementService, FileManagementService>();
        return services;
    }

    public static IServiceCollection ConfigureMessageBroker(this IServiceCollection services)
    {
        var mqttClientMock = new Mock<IMqttClient>();
        mqttClientMock.Setup(x => x.ConnectAsync(It.IsAny<MqttClientOptions>(), default))
            .ReturnsAsync(new MqttClientConnectResult());
        mqttClientMock.Setup(x => x.DisconnectAsync(new MqttClientDisconnectOptions(), default));
        var sampleSuccessfulResponse = new MqttClientPublishResult(15, MqttClientPublishReasonCode.Success, "Success",
            new List<MqttUserProperty>());
        mqttClientMock.Setup(x => x.PublishAsync(It.IsAny<MqttApplicationMessage>(), default))
            .ReturnsAsync(sampleSuccessfulResponse);
        var mqttClientImplementation = mqttClientMock.Object;
        services.AddSingleton<IMqttClient>(mqttClientImplementation);
        return services;
    }

    public static IServiceCollection ConfigureRequestPipelineServices(this IServiceCollection services)
    {
        services.AddSingleton<ExceptionHandlingMiddleware>();
        return services;
    }

    public static WebApplicationBuilder ConfigureSerilogLogging(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        Log.Logger = logger;
        builder.Host.UseSerilog(logger);
        return builder;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                "Idt.Profiles.Api.xml"));
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                "Idt.Profiles.Dto.xml"));
            options.ExampleFilters();
        });
        
        services.AddSwaggerExamplesFromAssemblyOf<ProfileExample>();
        
        return services;
    }

    public static IServiceCollection ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Profile>, ProfileValidator>();   
        services.AddScoped<IValidator<IFormFile>, ImageFileValidator>();
        services.AddScoped<IValidator<ProfileAddressCreateUpdateDto>, DummyAddressValidator>();
        return services;
    }
}