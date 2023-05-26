using Idt.Profiles.Persistence.Models;
using Idt.Profiles.Shared.ConfigurationOptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Idt.Profiles.Persistence.Repositories.ProfileImageRepository.Implementations;

public class ProfileImageInfoRepository : IProfileImageInfoRepository
{
    private readonly IMongoCollection<ProfileImageInfo> _profilesImagesCollection;

    public ProfileImageInfoRepository(IMongoClient mongoClient, IOptions<MongoDbConfigurationOptions> mongoDbConfiguration)
    {
        var database = mongoClient.GetDatabase(mongoDbConfiguration.Value.Database);
        _profilesImagesCollection =
            database.GetCollection<ProfileImageInfo>(mongoDbConfiguration.Value.ProfileImagesCollection);
    }

    public async Task<ProfileImageInfo?> GetProfileImageInfoAsync(Guid profileId)
    {
        FilterDefinition<ProfileImageInfo> profileIdFilter =
            Builders<ProfileImageInfo>.Filter.Eq(x => x.ProfileId, profileId);
        return await (await _profilesImagesCollection.FindAsync(profileIdFilter)).FirstOrDefaultAsync();
    }

    public async Task SaveProfileImageInfoAsync(ProfileImageInfo image)
    {
        FilterDefinition<ProfileImageInfo> profileIdFilter =
            Builders<ProfileImageInfo>.Filter.Eq(x => x.ProfileId, image.ProfileId);
        await _profilesImagesCollection.ReplaceOneAsync(profileIdFilter, image, new ReplaceOptions { IsUpsert = true });
    }

    public async Task DeleteProfileImageInfoAsync(Guid profileId)
    {
        var profileIdFilter = Builders<ProfileImageInfo>.Filter.Eq(x => x.ProfileId, profileId);
        await _profilesImagesCollection.DeleteOneAsync(profileIdFilter);
    }
}