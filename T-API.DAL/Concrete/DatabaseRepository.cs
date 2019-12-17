using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using T_API.Core.DAL.Abstract;
using T_API.Core.DAL.Concrete;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.DAL.Concrete
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;

        public DatabaseRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }


        public async Task<int> AddDatabase(DatabaseEntity database)
        {
            using var conn =await _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if (conn.State==ConnectionState.Broken||conn.State==ConnectionState.Closed) conn.Open();
            //user repo yu da yazak madem
            return Int32.MaxValue;
        }

        public Task UpdateDatabase(DatabaseEntity database)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteDatabase(DatabaseEntity database)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<DatabaseEntity>> GetByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<DatabaseEntity> GetById(int databaseId)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<DatabaseEntity>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task SuspendDatabase(int databaseId)
        {
            throw new System.NotImplementedException();
        }

        public Task RecoverDatabase(int databaseId)
        {
            throw new System.NotImplementedException();
        }
    }
}