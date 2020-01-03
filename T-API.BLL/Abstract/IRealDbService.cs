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
    public interface IRealDbService
    {
        Task CreateDatabaseOnRemote(AddDatabaseDto database);
        Task CreateTableOnRemote(AddTableDto database, DbInformation dbInformation);
        Task CreateColumnOnRemote(AddColumnDto column,DbInformation dbInformation);
        Task AlterColumnOnRemote(UpdateColumnDto column, DbInformation dbInformation);
        Task DropColumnOnRemote(DeleteColumnDto column, DbInformation dbInformation);

        Task CreateForeignKeyOnRemote(AddForeignKeyDto foreignKey, DbInformation dbInformation);
        Task AlterForeignKeyOnRemote(UpdateForeignKeyDto foreignKey, DbInformation dbInformation);
        Task DropForeignKeyOnRemote(DeleteForeignKeyDto foreignKey, DbInformation dbInformation);

        Task ExecuteQueryOnRemote(string query, DbInformation dbInformation);
        Task ExecuteQueryOnRemote(List<string> queries,DbInformation dbInformation);
        Task ExecuteQueryOnRemote(string query);


        Task<List<DetailTableDto>> GetTables(string databaseName,string provider);
        Task<DetailTableDto> GetTable(string tableName, string databaseName, string provider);





    }
}