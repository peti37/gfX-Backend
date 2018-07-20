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
    public class UserRepository : ICrudRepositories<Userke>
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Userke> _users;

        public UserRepository()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("gfX");
            _users = _database.GetCollection<Userke>("users");
        }

        public async Task Create(Userke user)
        {
            await _users.InsertOneAsync(user);
        }

        public Task Delete(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Userke>> FilterByField(string fieldName, string fieldValue)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Userke>> SelectAll()
        {
            return await _users.Find(new BsonDocument()).ToListAsync();
        }

        public Task<Userke> SelectById(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public Task Update(ObjectId id, string updateFieldName, string updateFieldValue)
        {
            throw new NotImplementedException();
        }
    }
}
