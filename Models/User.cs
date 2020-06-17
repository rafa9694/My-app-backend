using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace My_app_backend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Admin { get; set; }
    }
    
}

namespace My_app_backend.Models 
{
    public class UserDto 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public string Email { get; set; }

        public bool Admin { get; set; }
    }
}