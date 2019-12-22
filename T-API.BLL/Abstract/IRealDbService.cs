using System.Threading.Tasks;
using T_API.Entity.Concrete;

namespace T_API.BLL.Abstract
{
    public interface IRealDbService
    {
        Task CreateDatabaseOnRemote(Database database);
        Task CreateTableOnRemote(Database database);
        Task CreateColumnOnRemote(Database database);
    }
}