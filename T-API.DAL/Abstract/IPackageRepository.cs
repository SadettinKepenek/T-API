using System.Collections.Generic;
using System.Threading.Tasks;
using T_API.Entity.Concrete;

namespace T_API.DAL.Abstract
{
    public interface IPackageRepository
    {
        Task<List<DatabasePackage>> Get();
        Task<DatabasePackage> GetById(int id);
        Task<DatabasePackage> GetByName(string id);
        Task Add(DatabasePackage package);
        Task Update(DatabasePackage package);
        Task Delete(DatabasePackage package);
    }
}