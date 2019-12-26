using System.Collections.Generic;
using System.Threading.Tasks;
using T_API.Core.DAL.Concrete;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.DAL.Concrete
{
    public class MssqlRealDbRepository: IRealDbRepository
    {
        // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli

        public MssqlRealDbRepository()
        {
        }



        public Task ExecuteQueryOnRemote(string query, DbInformation dbInformation)
        {
            throw new System.NotImplementedException();
        }

        public Task ExecuteQueryOnRemote(string query)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Table>> GetTables(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public Task<Table> GetTable(string tableName, string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ForeignKey>> GetForeignKeys(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ForeignKey>> GetForeignKeys(string databaseName, string tableName)
        {
            throw new System.NotImplementedException();
        }

        public Task<ForeignKey> GetForeignKey(string databaseName, string tableName, string foreignKeyName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Key>> GetKeys(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Key>> GetKeys(string databaseName, string tableName)
        {
            throw new System.NotImplementedException();
        }

        public Task<Key> GetKey(string databaseName, string tableName, string keyName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Index>> GetIndices(string databaseName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Index>> GetIndices(string databaseName, string tableName)
        {
            throw new System.NotImplementedException();
        }

        public Task<Index> GetIndex(string databaseName, string tableName, string indexName)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Column>> GetColumns(string databaseName, string tableName)
        {
            throw new System.NotImplementedException();
        }

        public Task<Column> GetColumn(string databaseName, string tableName, string columnName)
        {
            throw new System.NotImplementedException();
        }
    }
}