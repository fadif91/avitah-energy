using MongoDB.Bson.Serialization.Attributes;

namespace Avitah.Energy.Service.Model
{
    [BsonIgnoreExtraElements]
    public class Data
    {
        [BsonElement("value")]
        public double Value { get; set; }
    }
}
