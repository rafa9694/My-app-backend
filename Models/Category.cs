using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace My_app_backend.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public string Path { get; set; }
    }
}