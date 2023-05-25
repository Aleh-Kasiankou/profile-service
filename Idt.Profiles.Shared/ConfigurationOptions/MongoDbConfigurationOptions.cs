namespace Idt.Profiles.Shared.ConfigurationOptions;

public class MongoDbConfigurationOptions
{
    public string ConnectionString { get; set; }
    public string Database { get; set; }
    public string ProfilesCollection { get; set; }
    public string ProfileImagesCollection { get; set; }
    public string EventOutboxCollection { get; set; }
}