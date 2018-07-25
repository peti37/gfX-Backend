using gfX.Interfaces;
using gfX.Models;
using gfX.Models.ViewModels;
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

        public async Task<List<UserDTO>> SelectAll()
        {
            List<UserDTO> DTOlist = new List<UserDTO>();
            var userList = await userRepo.SelectAll();
            for (int i = 0; i < userList.Count; i++)
            {
                DTOlist.Add(new UserDTO { GithubHandle = userList[i].GithubHandle, Name = userList[i].Name, Avatar = userList[i].Avatar, Email = userList[i].Email, HasJob = userList[i].HasJob });
            }

            return DTOlist;
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
