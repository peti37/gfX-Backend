using gfX.Interfaces;
using gfX.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gfX.Repositories
{
    public class GFXUserRepository : ICrudRepositories<GFXUser>
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<GFXUser> _users;

        public GFXUserRepository()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("gfX");
            _users = _database.GetCollection<GFXUser>("users");
        }

        public async Task<bool> CheckUser(string fieldValue)
        {
            var filter = Builders<GFXUser>.Filter.Eq("githubHandle", fieldValue);
            var result = await _users.Find(filter).ToListAsync();
            if (result.Count == 0)
            {
                return true;
            }
            
            return false;
        }

        public async Task Create(GFXUser user)
        {
            await _users.InsertOneAsync(user);
        }

        public Task Delete(ObjectId id)
        {
            throw new NotImplementedException();
        }


        public async Task<List<GFXUser>> FilterByField(FilterJson json)
        {
            var filter = Builders<GFXUser>.Filter.Eq(json.FieldName, json.FieldValue);
            var result = await _users.Find(filter).ToListAsync();

            return result;
        }

        public async Task<List<GFXUser>> SelectAll()
        {
            return await _users.Find(new BsonDocument()).ToListAsync();
        }

        public Task<GFXUser> SelectById(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public Task Update(ObjectId id, string updateFieldName, string updateFieldValue)
        {
            throw new NotImplementedException();
        }
    }
}
