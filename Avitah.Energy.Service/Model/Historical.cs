using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Avitah.Energy.Service.Model
{
    [BsonIgnoreExtraElements]
    public class Historical
    {
        [BsonElement("key")]
        public string Key { get; set; }

        [BsonElement("dateTime")]
        public DateTime DateTime { get; set; }

        [BsonElement("data")]
        public Data Data { get; set; }
    }
}
