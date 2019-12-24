using System.Collections.Generic;
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

        Task<List<DetailTableDto>> GetTables(string databaseName);
        Task<DetailTableDto> GetTable(string tableName, string databaseName);


        Task<List<DetailForeignKeyDto>> GetForeignKeys(string databaseName);
        Task<List<DetailForeignKeyDto>> GetForeignKeys(string databaseName, string tableName);
        Task<DetailForeignKeyDto> GetForeignKey(string databaseName, string tableName, string foreignKeyName);


        Task<List<DetailKeyDto>> GetKeys(string databaseName);
        Task<List<DetailKeyDto>> GetKeys(string databaseName, string tableName);
        Task<DetailKeyDto> GetKey(string databaseName, string tableName, string keyName);


        Task<List<DetailIndexDto>> GetIndices(string databaseName);
        Task<List<DetailIndexDto>> GetIndices(string databaseName, string tableName);
        Task<DetailIndexDto> GetIndex(string databaseName, string tableName, string indexName);


        Task<List<DetailColumnDto>> GetColumns(string databaseName, string tableName);
        Task<DetailColumnDto> GetColumn(string databaseName, string tableName, string columnName);




    }
}