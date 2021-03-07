using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avitah.Energy.Service.Model
{
    public class DataSource
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        [BsonElement("key")]
        public string Key { get; set; }

        [BsonElement("unit")]
        public string Unit { get; set; }

        [BsonConstructor]
        public DataSource(ObjectId id, string key, string unit)
        {
            Id = id;
            Key = key;
            Unit = unit;
        }
    }
}
