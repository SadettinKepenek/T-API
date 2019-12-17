using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using T_API.Core.DAL.Abstract;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.DAL.Concrete
{
    public class UserRepository : IUserRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> AddUser(UserEntity user)
        {
            using var conn =await _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if(conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();
            conn.BeginTransaction();



            var cmd = new SqlCommand("", conn as SqlConnection);

            throw new System.NotImplementedException();
        }

        public Task UpdateUser(UserEntity user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteUser(UserEntity user)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<UserEntity>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<UserEntity> GetById(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}