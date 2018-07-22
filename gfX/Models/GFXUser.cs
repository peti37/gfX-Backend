using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace gfX.Models
{
    public class GFXUser
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("githubHandle")]
        public string GithubHandle { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("hasJob")]
        public bool HasJob { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("repos")]
        public List<string> Repos { get; set; }
    }
}
