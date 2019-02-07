using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SCNDISC.Server.Domain.Aggregates.Partners;

namespace SCNDISC.Server.Domain.Aggregates
{
    public class Aggregate : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }

    public class Entity
    {
        public IEnumerable<LocalizableText> Name { get; set; }
    }
}
