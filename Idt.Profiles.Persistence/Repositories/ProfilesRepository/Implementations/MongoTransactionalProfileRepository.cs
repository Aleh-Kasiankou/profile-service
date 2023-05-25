using Idt.Profiles.Persistence.Events;
using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Shared.ConfigurationOptions;
using Idt.Profiles.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Polly.Wrap;

namespace Idt.Profiles.Persistence.Repositories.ProfilesRepository.Implementations;

public class MongoTransactionalProfileRepository : IProfileRepository
{
    private readonly IMongoCollection<Profile> _profilesCollection;
    private readonly IMongoCollection<OutboxEvent> _eventsCollection;
    private readonly ILogger<MongoTransactionalProfileRepository> _logger;
    private readonly Guid _rollbackId = Guid.NewGuid();
    private PolicyWrap _retryRollbackPolicy;

    public MongoTransactionalProfileRepository(IOptions<MongoDbConfigurationOptions> mongoDbConfiguration,
        ILogger<MongoTransactionalProfileRepository> logger)
    {
        _logger = logger;
        var mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
        var database = mongoClient.GetDatabase(mongoDbConfiguration.Value.Database);
        _profilesCollection = database.GetCollection<Profile>(mongoDbConfiguration.Value.ProfilesCollection);
        _eventsCollection = database.GetCollection<OutboxEvent>(mongoDbConfiguration.Value.EventOutboxCollection);

        // TODO OPTIMIZE. Perhaps pass rollback policy from above and use config? 
        CreateUniqueUsernameIndex();
        CreateRollbackRetryPolicy();
    }

    public async Task<Profile> GetProfileAsync(Guid profileId)
    {
        var idFilter = Builders<Profile>.Filter.Eq(x => x.ProfileId, profileId);
        var savedProfile = await (await _profilesCollection.FindAsync<Profile>(idFilter))
            .FirstOrDefaultAsync();
        return savedProfile ?? throw new ProfileDoesNotExistException(profileId);
    }

    public async Task CreateProfileAsync(Profile profile)
    {
        Guid profileId = Guid.Empty;
        try
        {
            await _profilesCollection.InsertOneAsync(profile);
            profileId = profile.ProfileId;
            var eventToSend = new OutboxEvent(new ProfileCreatedEvent(profile));
            await _eventsCollection.InsertOneAsync(eventToSend);
        }
        catch
        {
            var profileIsSavedToDatabase = profileId != Guid.Empty;
            if (profileIsSavedToDatabase)
            {
                await _retryRollbackPolicy.Execute(async () => await RollbackProfileCreation(profileId));
                return;
            }

            throw;
        }
    }

    public async Task UpdateProfileAsync(Profile modifiedProfile, Profile savedProfile, bool isRollback = false)
    {
        try
        {
            FilterDefinition<Profile> filter;
            if (isRollback)
            {
                filter = Builders<Profile>.Filter.Eq(x => x.ProfileId, modifiedProfile.ProfileId);
            }
            else
            {
                modifiedProfile.ConcurrencyMarker = Guid.NewGuid();
                filter = BuildProfileIdFilterWithOptimisticLock(modifiedProfile.ProfileId,
                    savedProfile.ConcurrencyMarker);
            }

            var replaceResult = await _profilesCollection.ReplaceOneAsync(filter, modifiedProfile);
            if (!isRollback && replaceResult.ModifiedCount != 1)
            {
                throw new ProfileConcurrentUpdateFailedException(modifiedProfile.ProfileId);
            }

            var eventToSend = new OutboxEvent(new ProfileUpdatedEvent(modifiedProfile));
            await _eventsCollection.InsertOneAsync(eventToSend);
        }
        catch (ProfileConcurrentUpdateFailedException)
        {
            throw;
        }
        catch
        {
            await _retryRollbackPolicy.Execute(async () => await RollbackProfileUpdate(savedProfile));
        }
    }

    public async Task DeleteProfileAsync(Guid profileId)
    {
        var savedProfile = await GetProfileAsync(profileId);

        try
        {
            var idFilter = Builders<Profile>.Filter.Eq(x => x.ProfileId, profileId);
            await _profilesCollection.DeleteOneAsync(idFilter);
            var eventToSend = new OutboxEvent(new ProfileDeletedEvent(savedProfile));
            await _eventsCollection.InsertOneAsync(eventToSend);
        }
        catch
        {
            await _retryRollbackPolicy.Execute(async () => await RollbackProfileDeletion(savedProfile));
        }
    }

    private void CreateUniqueUsernameIndex()
    {
        var profileIndexKeys = Builders<Profile>.IndexKeys;
        var indexModel = new CreateIndexModel<Profile>(profileIndexKeys.Ascending(x => x.UserName),
            new CreateIndexOptions { Unique = true });
        _profilesCollection.Indexes.CreateOne(indexModel);
    }

    private void CreateRollbackRetryPolicy()
    {
        // TODO CONFIGURE INTERVALS
        var retryRollbackPolicy = Policy
            .Handle<RollbackFailedException>()
            .Retry(3);

        var fallBackRetryPolicy = Policy
            .Handle<RollbackFailedException>()
            .Fallback(() =>
                _logger.LogCritical(
                    "Failed to rollback transaction. Please check the rollback logs with RollbackId : {RollbackId}",
                    _rollbackId));

        _retryRollbackPolicy = Policy.Wrap(retryRollbackPolicy, fallBackRetryPolicy);
    }

    private FilterDefinition<Profile> BuildProfileIdFilterWithOptimisticLock(Guid profileId, Guid ConcurrencyMarker)
    {
        var idFilter = Builders<Profile>.Filter.Eq(x => x.ProfileId, profileId);
        var modifiedAtFilter = Builders<Profile>.Filter.Eq(x => x.ConcurrencyMarker, ConcurrencyMarker);
        var idFilterWithOptimisticLock = Builders<Profile>.Filter.And(idFilter, modifiedAtFilter);
        return idFilterWithOptimisticLock;
    }

    private async Task RollbackProfileCreation(Guid profileId)
    {
        try
        {
            _logger.LogInformation("Starting rollback with RollbackId {RollbackId}", _rollbackId);
            await DeleteProfileAsync(profileId);
            _logger.LogInformation("Finishing rollback with RollbackId {RollbackId}", _rollbackId);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e,
                "Rollback with RollbackId {RollbackId} failed. Profile with ProfileId {ProfileId} needs to be deleted",
                _rollbackId, profileId);
            throw new RollbackFailedException($"Failed to rollback creation of profile with id {profileId}");
        }

        throw new ProfileCreationTransactionRolledBackException();
    }

    private async Task RollbackProfileUpdate(Profile previousProfileState)
    {
        try
        {
            _logger.LogInformation("Starting rollback with RollbackId {RollbackId}", _rollbackId);
            var currentProfileState = await GetProfileAsync(previousProfileState.ProfileId);
            if (!currentProfileState.Equals(previousProfileState))
            {
                await UpdateProfileAsync(previousProfileState, currentProfileState, true);
            }

            _logger.LogInformation("Finishing rollback with RollbackId {RollbackId}", _rollbackId);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e,
                "Rollback with RollbackId {RollbackId} failed. Profile with ProfileId {ProfileId}" +
                " needs to be restored to the following state : {@ProfileState}",
                _rollbackId, previousProfileState.ProfileId, previousProfileState);
            throw new RollbackFailedException($"Failed to rollback update of profile with id {previousProfileState}");
        }
    }

    private async Task RollbackProfileDeletion(Profile profile)
    {
        try
        {
            _logger.LogInformation("Starting rollback with RollbackId {RollbackId}", _rollbackId);
            await CreateProfileAsync(profile);
            _logger.LogInformation("Finishing rollback with RollbackId {RollbackId}", _rollbackId);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Rollback with RollbackId {RollbackId} failed. Profile with ProfileId {ProfileId}" +
                                  " needs to be restored to the following state : {@ProfileState}", _rollbackId,
                profile.ProfileId, profile);
            throw new RollbackFailedException($"Failed to rollback deletion of profile with id {profile.ProfileId}");
        }
    }
}