using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using T_API.Entity.Concrete;

namespace T_API.DAL.Abstract
{
    public interface IDatabaseRepository
    {
        Task<int> AddDatabase(DatabaseEntity database);
        Task UpdateDatabase(DatabaseEntity database);
        Task DeleteDatabase(DatabaseEntity database);
        Task<List<DatabaseEntity>> GetByUser(int userId);
        Task<DatabaseEntity> GetById(int databaseId);
        Task<List<DatabaseEntity>> GetAll();
        Task SuspendDatabase(int databaseId);
        Task RecoverDatabase(int databaseId);

    }
}