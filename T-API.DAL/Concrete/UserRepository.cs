using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using T_API.Core.DAL.Abstract;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.DAL.Concrete
{
    public class UserRepository : IUserRepository
    {
        // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli

        private IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> AddUser(UserEntity user)
        {
            try
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
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }



        }

        public async Task UpdateUser(UserEntity user)
        {

            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql = "Update users set Username = @username, Password = @password,Firstname = @firstname,Lastname = @lastname,Role = @role,PhoneNumber = @phonenumber,Balance = @balance," +
                             "IsActive = @isActive where UserId = @userId";
                
                using var cmd = new MySqlCommand(sql, conn as MySqlConnection);

                cmd.Parameters.AddWithValue("userId", user.UserId);
                cmd.Parameters.AddWithValue("username", user.Username);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("firstname", user.Firstname);
                cmd.Parameters.AddWithValue("lastname", user.Lastname);
                cmd.Parameters.AddWithValue("role", user.Role);
                cmd.Parameters.AddWithValue("phonenumber", user.PhoneNumber);
                cmd.Parameters.AddWithValue("balance", user.Balance);
                cmd.Parameters.AddWithValue("isActive", user.IsActive);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task DeleteUser(UserEntity user)
        {
            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql = "Delete from users where UserId = @UserId";
                using var cmd = new MySqlCommand(sql, conn as MySqlConnection);

                cmd.Parameters.AddWithValue("UserId", user.UserId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<List<UserEntity>> GetAll()
        {
            try
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
                                UserEntity user = ProcessUserEntity(row);

                                users.Add(user);
                            }
                            return users;
                        }

                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        public async Task<UserEntity> GetById(int userId)
        {

            try
            {
                using (var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation))
                {

                    if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();
                    string sql = "Select * from users where UserId=@UserId";
                    using (var cmd = new MySqlCommand(sql, conn as MySqlConnection))
                    {
                        cmd.Parameters.AddWithValue("UserId", userId);
                        using var sqlReader = cmd.ExecuteReader();
                        if (!sqlReader.HasRows)
                        {
                            throw new NullReferenceException($"{userId} kullanıcısının verilerine ulaşılamadı");
                        }

                        //sqlReader.Read();
                        DataTable dt = new DataTable();
                        dt.Load(sqlReader);

                        DataRow dr = dt.Rows[0];
                        var userEntity = ProcessUserEntity(dr);
                        return userEntity;
                    }

                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        private static UserEntity ProcessUserEntity(DataRow dr)
        {
            string username = dr["Username"] as string;
            string password = dr["Password"] as string;
            string firstname = dr["Firstname"] as string;
            string lastname = dr["Lastname"] as string;
            string role = dr["Role"] as string;
            string email = dr["Email"] as string;
            string phoneNumber = dr["PhoneNumber"] as string;
            decimal balance = Convert.ToDecimal(dr["Balance"]);
            bool isActive = Convert.ToBoolean(dr["IsActive"]);
            var Id = (int) dr["UserId"];


            var userEntity = new UserEntity
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
                UserId = Id
            };
            return userEntity;
        }

        public async Task<UserEntity> GetByUsername(string username)
        {
            try
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
                        return ProcessUserEntity(dr);
                    }

                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }
    }
}