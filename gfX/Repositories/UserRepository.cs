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
    public class UserRepository : ICrudRepositories<User>
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<User> _users;

        public UserRepository()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("gfX");
            _users = _database.GetCollection<User>("users");
        }

        public async Task Create(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public Task Delete(ObjectId id)
        {
            throw new NotImplementedException();
        }


        public async Task<List<User>> FilterByField(FilterJson json)
        {
            var filter = Builders<User>.Filter.Eq(json.FieldName, json.FieldValue);
            var result = await _users.Find(filter).ToListAsync();

            return result;
        }

        public async Task<List<User>> SelectAll()
        {
            return await _users.Find(new BsonDocument()).ToListAsync();
        }

        public Task<User> SelectById(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public Task Update(ObjectId id, string updateFieldName, string updateFieldValue)
        {
            throw new NotImplementedException();
        }
    }
}
