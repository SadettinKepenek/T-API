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
            using var cmd=new SqlCommand("",conn as SqlConnection);
            string sql =
                "Insert into users (Firstname,Lastname,Email,PhoneNumber,Balance,Username,Password,IsActive ) Values (@Firstname , @Lastname , " +
                "@Email , @PhoneNumber , @Balance , @Username , @Password , @IsActive)";

            cmd.Parameters.AddWithValue("Firstname", user.Firstname);
            cmd.Parameters.AddWithValue("Lastname", user.Lastname);
            cmd.Parameters.AddWithValue("Email", user.Email);
            cmd.Parameters.AddWithValue("PhoneNumber", user.PhoneNumber);
            cmd.Parameters.AddWithValue("Balance", user.Balance);
            cmd.Parameters.AddWithValue("Username", user.Username);
            cmd.Parameters.AddWithValue("Password", user.Password);
            cmd.Parameters.AddWithValue("IsActive", user.IsActive);
            var id = (int)await cmd.ExecuteScalarAsync(); 

            return id;
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