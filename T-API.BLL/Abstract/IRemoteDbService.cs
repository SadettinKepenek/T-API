using System.Collections.Generic;
using System.Threading.Tasks;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
using T_API.Core.DTO.Table;
using T_API.Entity.Concrete;

namespace T_API.BLL.Abstract
{
    public interface IRemoteDbService
    {
        Task CreateDatabaseOnRemote(AddDatabaseDto database);
        Task CreateTableOnRemote(AddTableDto database, DbInformation dbInformation);
        Task DropTableOnRemote(DeleteTableDto table, DbInformation dbInformation);
        Task CreateKeyOnRemote(AddKeyDto key, DbInformation dbInformation);
        Task AlterKeyOnRemote(UpdateKeyDto key, DbInformation dbInformation);
        Task CreateColumnOnRemote(AddColumnDto column,DbInformation dbInformation);
        Task AlterColumnOnRemote(UpdateColumnDto column, DbInformation dbInformation);
        Task DropColumnOnRemote(DeleteColumnDto column, DbInformation dbInformation);
        Task DropKeyOnRemote(DeleteKeyDto key, DbInformation dbInformation);
        Task CreateForeignKeyOnRemote(AddForeignKeyDto foreignKey, DbInformation dbInformation);
        Task AlterForeignKeyOnRemote(UpdateForeignKeyDto foreignKey, DbInformation dbInformation);
        Task DropForeignKeyOnRemote(DeleteForeignKeyDto foreignKey, DbInformation dbInformation);

        Task ExecuteQueryOnRemote(string query, DbInformation dbInformation);
        Task ExecuteQueryOnRemote(List<string> queries,DbInformation dbInformation);
        Task ExecuteQueryOnRemote(string query);


        Task<List<DetailTableDto>> GetTables(DbInformation dbInformation);
        Task<DetailTableDto> GetTable(string tableName, DbInformation dbInformation);

        Task<List<string>> GetAvailableProviders();
        Task<DbInformation> GetAvailableServer(string provider);
        Task<string> GenerateDatabaseName(int userId);
        Task<string> GenerateUserName(int userId);
        Task<string> GeneratePassword(int userId);





    }
}