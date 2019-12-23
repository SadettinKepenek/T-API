using System.Threading.Tasks;
using T_API.Core.DTO.Database;
using T_API.Entity.Concrete;

namespace T_API.BLL.Abstract
{
    public interface IRealDbService
    {
        Task CreateDatabaseOnRemote(AddDatabaseDto database);
        Task CreateTableOnRemote(Database database);
        Task CreateColumnOnRemote(Database database);
    }
}