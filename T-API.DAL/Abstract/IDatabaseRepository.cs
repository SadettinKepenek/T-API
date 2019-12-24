using System.Collections.Generic;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using T_API.Entity.Concrete;

namespace T_API.DAL.Abstract
{
    public interface IDatabaseRepository
    {
        Task<int> AddDatabase(Database database);
        Task UpdateDatabase(Database database);
        Task DeleteDatabase(Database database);
        Task<List<Database>> GetByUser(int userId);
        Task<List<Database>> GetByUser(string username);
        Task<Database> GetById(int databaseId);
        Task<List<Database>> GetAll();
        Task SuspendDatabase(int databaseId);
        Task RecoverDatabase(int databaseId);

    }
}