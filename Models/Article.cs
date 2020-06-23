using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace My_app_backend.Models
{
    public class Article
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Category_Id { get; set; }

        public string Content { get; set; }

    }
}