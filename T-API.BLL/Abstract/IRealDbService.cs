using System.Threading.Tasks;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
using T_API.Core.DTO.Table;
using T_API.Entity.Concrete;

namespace T_API.BLL.Abstract
{
    public interface IRealDbService
    {
        Task CreateDatabaseOnRemote(AddDatabaseDto database);
        Task CreateTableOnRemote(AddTableDto database);
        Task CreateColumnOnRemote(AddColumnDto column);
        Task CreateIndexOnRemote(AddIndexDto index);
        Task CreateForeignKeyOnRemote(AddForeignKeyDto foreignKey);
        Task CreateKeyOnRemote(AddKeyDto key);
    }
}