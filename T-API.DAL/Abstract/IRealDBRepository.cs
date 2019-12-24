using System.Globalization;
using System.Threading.Tasks;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;

namespace T_API.DAL.Abstract
{
    public interface IRealDbRepository
    {
        Task CreateDatabaseOnRemote(string query);
        Task CreateTableOnRemote(string query);
        Task CreateColumnOnRemote(string query);

        Task CreateIndexOnRemote(string query);
        Task CreateForeignKeyOnRemote(string query);
        Task CreateKeyOnRemote(string query);
    }
}