using MongoDB.Bson.Serialization.Attributes;

namespace Avitah.Energy.Service.Model
{
    public class Mapping
    {
        [BsonElement("key")]
        public string Key { get; set; }

        [BsonElement("field")]
        public string Field { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("quantity")]
        public string Quantity { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonConstructor]
        public Mapping(string key, string field, string country, string quantity, string type, string category)
        {
            Key = key;
            Field = field;
            Country = country;
            Quantity = quantity;
            Type = type;
            Category = category;
        }
    }
}
