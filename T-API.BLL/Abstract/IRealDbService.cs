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
        Task CreateTableOnRemote(AddTableDto database);
        Task CreateColumnOnRemote(AddColumnDto column,DbInformation dbInformation);
        Task CreateIndexOnRemote(AddIndexDto index);
        Task CreateForeignKeyOnRemote(AddForeignKeyDto foreignKey);
        Task CreateKeyOnRemote(AddKeyDto key);

        Task<List<DetailTableDto>> GetTables(string databaseName,string provider);
        Task<DetailTableDto> GetTable(string tableName, string databaseName, string provider);


        Task<List<DetailForeignKeyDto>> GetForeignKeys(string databaseName, string provider);
        Task<List<DetailForeignKeyDto>> GetForeignKeys(string databaseName, string tableName, string provider);
        Task<DetailForeignKeyDto> GetForeignKey(string databaseName, string tableName, string foreignKeyName, string provider);


        Task<List<DetailKeyDto>> GetKeys(string databaseName, string provider);
        Task<List<DetailKeyDto>> GetKeys(string databaseName, string tableName, string provider);
        Task<DetailKeyDto> GetKey(string databaseName, string tableName, string keyName, string provider);


        Task<List<DetailIndexDto>> GetIndices(string databaseName, string provider);
        Task<List<DetailIndexDto>> GetIndices(string databaseName, string tableName, string provider);
        Task<DetailIndexDto> GetIndex(string databaseName, string tableName, string indexName, string provider);


        Task<List<DetailColumnDto>> GetColumns(string databaseName, string tableName, string provider);
        Task<DetailColumnDto> GetColumn(string databaseName, string tableName, string columnName, string provider);




    }
}