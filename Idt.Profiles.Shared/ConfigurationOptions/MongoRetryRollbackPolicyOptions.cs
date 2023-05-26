namespace Idt.Profiles.Shared.ConfigurationOptions;

public class MongoRetryRollbackPolicyOptions
{
    public int NumberOfRetryRollbackAttempts { get; set; }
    public int MillisecondsBetweenRetries { get; set; }
}