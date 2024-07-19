using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace drawIT.Models
{
    public class AWSService
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Name { get; set; }
        public string? Category { get; set; }
    }
}
