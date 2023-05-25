using MongoDB.Bson.Serialization.Attributes;

namespace Idt.Profiles.Persistence.Models;

public class ProfileImageInfo
{
    [BsonId]
    public Guid ProfileId { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
}