using gfX.Models;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gfX.Interfaces
{
    public interface ICrudRepositories<T> where T : class
    {
        Task Create(T entity);
        Task<T> SelectById(ObjectId id);
        Task<List<T>> SelectAll();
        Task<List<T>> FilterByField(FilterJson json);
        Task<bool> CheckUser(string fieldValue);
        Task Update(GFXUser user, FilterJson updateData);
        Task Delete(ObjectId id);
        Task<List<Repo>> EachRepo(string token);
        Task<List<string>> Orgsozas(string token);

    }
}
