using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
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
            using (var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation))
            {
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();


                string sql =
                    "Insert into users (Firstname,Lastname,Email,PhoneNumber,Balance,Username,Password,IsActive ) Values (@Firstname , @Lastname , " +
                    "@Email , @PhoneNumber , @Balance , @Username , @Password , @IsActive); SELECT LAST_INSERT_ID();";

                using (var cmd = new MySqlCommand("", conn as MySqlConnection))
                {

                    cmd.CommandText = sql;
                    cmd.Connection = conn as MySqlConnection;
                    cmd.Parameters.AddWithValue("Firstname", user.Firstname);
                    cmd.Parameters.AddWithValue("Lastname", user.Lastname);
                    cmd.Parameters.AddWithValue("Email", user.Email);
                    cmd.Parameters.AddWithValue("PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("Balance", user.Balance);
                    cmd.Parameters.AddWithValue("Username", user.Username);
                    cmd.Parameters.AddWithValue("Password", user.Password);
                    cmd.Parameters.AddWithValue("IsActive", user.IsActive);
                    var id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;

                }
            }



        }

        public Task UpdateUser(UserEntity user)
        {

            throw new System.NotImplementedException();
        }

        public Task DeleteUser(UserEntity user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<UserEntity>> GetAll()
        {
            using (var conneciton = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation))
            {
                if (conneciton.State == ConnectionState.Broken || conneciton.State == ConnectionState.Closed) conneciton.Open();
                string sql = "Select * from users";
                using (var command = new MySqlCommand(sql, conneciton as MySqlConnection))
                {
                    using var sqlReader = command.ExecuteReader();
                    if (!sqlReader.HasRows)
                    {
                        throw new NullReferenceException(" kullanıcısının verilerine ulaşılamadı");
                    }
                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    if (dt.Rows.Count != 0)
                    {
                        List<UserEntity> users = new List<UserEntity>();
                        foreach (DataRow row in dt.Rows)
                        {
                            UserEntity user = new UserEntity();
                            user.UserId = (int)row["UserId"];
                            user.Password = row["Password"] as string;
                            user.Firstname = row["Firstname"] as string;
                            user.Lastname = row["Lastname"] as string;
                            user.Role = row["Role"] as string;
                            user.Email = row["Email"] as string;
                            user.PhoneNumber = row["PhoneNumber"] as string;
                            user.Balance = Convert.ToDecimal(row["Balance"]);
                            user.IsActive = Convert.ToBoolean(row["IsActive"]);

                            users.Add(user);
                        }
                        return users;
                    }

                }
            }
            return null;
        }

        public Task<UserEntity> GetById(int userId)
        {

            throw new System.NotImplementedException();
        }

        public async Task<UserEntity> GetByUsername(string username)
        {
            using (var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation))
            {

                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();
                string sql = "Select * from users where Username=@Username";
                using (var cmd = new MySqlCommand(sql, conn as MySqlConnection))
                {
                    cmd.Parameters.AddWithValue("Username", username);
                    using var sqlReader = cmd.ExecuteReader();
                    if (!sqlReader.HasRows)
                    {
                        throw new NullReferenceException($"{username} kullanıcısının verilerine ulaşılamadı");
                    }

                    //sqlReader.Read();
                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);

                    DataRow dr = dt.Rows[0];
                    int userId = (int)dr["UserId"];
                    string password = dr["Password"] as string;
                    string firstname = dr["Firstname"] as string;
                    string lastname = dr["Lastname"] as string;
                    string role = dr["Role"] as string;
                    string email = dr["Email"] as string;
                    string phoneNumber = dr["PhoneNumber"] as string;
                    decimal balance = Convert.ToDecimal(dr["Balance"]);
                    bool isActive = Convert.ToBoolean(dr["IsActive"]);

                    return new UserEntity
                    {
                        Email = email,
                        PhoneNumber = phoneNumber,
                        Username = username,
                        IsActive = isActive,
                        Balance = balance,
                        Lastname = lastname,
                        Password = password,
                        Firstname = firstname,
                        Role = role,
                        UserId = userId
                    };
                }

            }
        }
    }
}