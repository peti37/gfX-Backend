using gfX.Interfaces;
using gfX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gfX.Services
{
    public class GFXUserService
    {
        private ICrudRepositories<GFXUser> userRepo;

        public GFXUserService(ICrudRepositories<GFXUser> userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<List<GFXUser>> SelectAll()
        {
            return await userRepo.SelectAll();
        }

        public async Task Create(GFXUser user)
        {
            await userRepo.Create(user);
        }

        public async Task<bool> CheckUser(string fieldValue)
        {
            return await userRepo.CheckUser(fieldValue);
        }

        public async Task<List<string>> EachRepo(string token)
        {
            return await userRepo.EachRepo(token);
        }

        public async Task<List<string>> Orgsozas(string token)
        {
            return await userRepo.Orgsozas(token);
        }

        public async Task<List<GFXUser>> FilterByField(FilterJson json)
        {
            return await userRepo.FilterByField(json);
        }

    }
}
