using gfX.Entities;
using gfX.Interfaces;
using gfX.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Octokit.Internal;
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

        public GFXUserRepository(IOptions<AppSettings> appSettings)
        {
            _client = new MongoClient(appSettings.Value.ConnectionString);
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

        public async Task Update(GFXUser user, FilterJson updateData)
        {
            var update = Builders<GFXUser>.Update.Set(updateData.FieldName, updateData.FieldValue);
            var filter = Builders<GFXUser>.Filter.Eq("_id", user.Id);
            var result = await _users.UpdateOneAsync(filter, update);
        }

        public async Task<List<Repo>> EachRepo(string token)
        {
            var github = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("fasz"), new InMemoryCredentialStore(new Octokit.Credentials(token)));
            var Repositories = await github.Repository.GetAllForCurrent();

            List<Repo> returnList = new List<Repo>();

            for (int i = 0; i < Repositories.Count; i++)
            {
                returnList.Add(new Repo { Url = Repositories[i].HtmlUrl });
            }
            return returnList;
        }

        public async Task<List<string>> Orgsozas(string token)
        {
            var github = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("fasz"), new InMemoryCredentialStore(new Octokit.Credentials(token)));
            var Orgsok = await github.Organization.GetAllForCurrent();

            List<string> returnList = new List<string>();

            for (int i = 0; i < Orgsok.Count; i++)
            {
                returnList.Add(Orgsok[i].Login);
            }
            return returnList;
        }
    }
}
