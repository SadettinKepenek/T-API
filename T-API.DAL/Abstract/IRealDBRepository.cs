using System.Globalization;
using System.Threading.Tasks;

namespace T_API.DAL.Abstract
{
    public interface IRealDbRepository
    {
        Task CreateDatabaseOnRemote(string query);
        Task CreateTableOnRemote(string query);
        Task CreateColumnOnRemote(string query);
    }
}