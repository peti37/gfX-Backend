using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gfX.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("githubHandle")]
        public string GithubHandle { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("hasJob")]
        public bool HasJob { get; set; }
        [BsonElement("repos")]
        public List<string> Repos { get; set; }
    }
}
