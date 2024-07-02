using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace drawIT.Models
{
    public class AWSService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Name { get; set; }
    }
}
