﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
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
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if (conn.State==ConnectionState.Broken||conn.State==ConnectionState.Closed) conn.Open();
            
            string sql =
                "Insert into `databases` (UserId,Server,Username,Password,Port,Provider,StartDate,EndDate,IsActive,IsStorageSupport,IsApiSupport)" + 
                "Values(@UserId,@Server,@Username,@Password,@Port,@Provider,@StartDate,@EndDate,@IsActive,@IsStorageSupport,@IsApiSupport ) ";
            using var cmd=new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("UserId", database.UserId);
            cmd.Parameters.AddWithValue("Server", database.Server);
            cmd.Parameters.AddWithValue("Username", database.Username);
            cmd.Parameters.AddWithValue("Password", database.Password);
            cmd.Parameters.AddWithValue("Port", database.Port);
            cmd.Parameters.AddWithValue("Provider", database.Provider);
            cmd.Parameters.AddWithValue("StartDate", database.StartDate);
            cmd.Parameters.AddWithValue("EndDate", database.EndDate);
            cmd.Parameters.AddWithValue("IsActive", database.IsActive);
            cmd.Parameters.AddWithValue("IsStorageSupport", database.IsStorageSupport);
            cmd.Parameters.AddWithValue("IsApiSupport", database.IsApiSupport);

            var id = (int)await cmd.ExecuteScalarAsync(); 
            

            return id;
        }

        public async Task UpdateDatabase(DatabaseEntity database)
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if(conn.State == ConnectionState.Broken||conn.State == ConnectionState.Closed) conn.Open();

            string sql =
                "Update `databases` Set UserId = @UserId,Server = @Server,Username = @Username,Password = @Password,Port = @Port,Provider = @Provider," +
                "StartDate = @StartDate,EndDate = @EndDate,IsActive = @IsActive,IsStorageSupport = @IsStorageSupport,IsApiSupport = @IsApiSupport where DatabaseId = @DatabaseId";
            using var cmd = new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            
            cmd.Parameters.AddWithValue("UserId", database.UserId);
            cmd.Parameters.AddWithValue("Server", database.Server);
            cmd.Parameters.AddWithValue("Username", database.Username);
            cmd.Parameters.AddWithValue("Password", database.Password);
            cmd.Parameters.AddWithValue("Port", database.Port);
            cmd.Parameters.AddWithValue("Provider", database.Provider);
            cmd.Parameters.AddWithValue("StartDate", database.StartDate);
            cmd.Parameters.AddWithValue("EndDate", database.EndDate);
            cmd.Parameters.AddWithValue("IsActive", database.IsActive);
            cmd.Parameters.AddWithValue("IsStorageSupport", database.IsStorageSupport);
            cmd.Parameters.AddWithValue("IsApiSupport", database.IsApiSupport);
            cmd.Parameters.AddWithValue("DatabaseId", database.DatabaseId);


        }

        public async Task DeleteDatabase(DatabaseEntity database)
        {

            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if(conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

            string sql = "Delete from `databases` where DatabaseId = @DatabaseId";
            using var cmd = new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("DatabaseId", database.DatabaseId);

        }

        public async Task<List<DatabaseEntity>> GetByUser(int userId)
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if(conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

            string sql = "Select * from `databases` where UserId = @UserId";
            using var cmd = new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("UserId", userId);

            var GetDatabaseByUser = (List<DatabaseEntity>) await cmd.ExecuteScalarAsync();

            return GetDatabaseByUser;

        }

        public async Task<List<DatabaseEntity>> GetByUser(string username)
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

            string sql = "Select * from `databases` inner join users on databases.UserId = users.UserId Where users.Username = @Username";
            using var cmd = new MySqlCommand("sql", conn as MySqlConnection);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("Username", username);


            var sqlReader = cmd.ExecuteReader();
            if (!sqlReader.HasRows)
            {
                return null;
            }

            DataTable dt=new DataTable();
            dt.Load(sqlReader);
            List<DatabaseEntity> databases=new List<DatabaseEntity>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                databases.Add(new DatabaseEntity{
                    Username = dt.Rows[i]["Username"] as string,
                    Provider = dt.Rows[i]["Provider"] as string,
                    Port = dt.Rows[i]["Port"] as string,
                    Password = dt.Rows[i]["Password"] as string,
                    StartDate = Convert.ToDateTime(dt.Rows[i]["StartDate"]),
                    Database = dt.Rows[i]["Database"] as string,
                    IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"]),
                    IsApiSupport = Convert.ToBoolean(dt.Rows[i]["IsApiSupport"]),
                    IsStorageSupport = Convert.ToBoolean(dt.Rows[i]["IsStorageSupport"]),
                    EndDate = Convert.ToDateTime(dt.Rows[i]["EndDate"]),
                    DatabaseId = Convert.ToInt32(dt.Rows[i]["DatabaseId"]),
                    Server = dt.Rows[i]["Server"] as string,
                    UserId = Convert.ToInt32(dt.Rows[i]["UserId"]),
                });
            }

            return databases;

        }

        public async Task<DatabaseEntity> GetById(int databaseId)
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

            string sql = "Select * from `databases` where DatabaseId = @DatabaseId";
            using var cmd = new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("DatabaseId", databaseId);

            var GetDatabaseById = (DatabaseEntity) await cmd.ExecuteScalarAsync();

            return GetDatabaseById;

        }

        public async Task<List<DatabaseEntity>> GetAll()
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

            string sql = "Select * from `databases`";
            using var cmd = new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            var GetAllDatabase = (List<DatabaseEntity>) await cmd.ExecuteScalarAsync();

            return GetAllDatabase;

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