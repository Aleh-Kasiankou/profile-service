using Idt.Profiles.Persistence.Models.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Idt.Profiles.Persistence.Models;

public class Profile : IModifiableAggregate
{
    [BsonId]
    public Guid ProfileId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ProfileAddress ProfileAddress { get; set; }
    public ProfileImageInfo? ProfileImage { get; set; }
    public Guid ConcurrencyMarker { get; set; } = Guid.NewGuid();
}