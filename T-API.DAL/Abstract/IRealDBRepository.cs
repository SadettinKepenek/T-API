using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using T_API.Core.DAL.Concrete;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
using T_API.Core.DTO.Table;
using T_API.Entity.Concrete;

namespace T_API.DAL.Abstract
{
    public interface IRealDbRepository
    {

        Task ExecuteQueryOnRemote(string query,DbInformation dbInformation);
        Task ExecuteQueryOnRemote(string query);

        Task<DataTable> Get(string query, DbInformation dbInformation);

        Task<List<Table>> GetTables(DbInformation dbInformation);
        Task<Table> GetTable(string tableName, DbInformation dbInformation);


        Task<List<ForeignKey>> GetForeignKeys(string databaseName);
        Task<List<ForeignKey>> GetForeignKeys(string databaseName, string tableName);
        Task<ForeignKey> GetForeignKey(string databaseName, string tableName, string foreignKeyName);


        Task<List<Key>> GetKeys(string databaseName);
        Task<List<Key>> GetKeys(string databaseName, string tableName);
        Task<Key> GetKey(string databaseName, string tableName, string keyName);


        Task<List<Index>> GetIndices(string databaseName);
        Task<List<Index>> GetIndices(string databaseName, string tableName);
        Task<Index> GetIndex(string databaseName, string tableName, string indexName);


        Task<List<Column>> GetColumns(string databaseName, string tableName);
        Task<Column> GetColumn(string databaseName, string tableName, string columnName);
    }
}