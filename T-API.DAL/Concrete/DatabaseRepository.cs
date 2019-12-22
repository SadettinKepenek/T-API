﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using T_API.Core.DAL.Abstract;
using T_API.Core.DAL.Concrete;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;

namespace T_API.DAL.Concrete
{
    // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli
    public class DatabaseRepository : IDatabaseRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;


        public DatabaseRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }


        public async Task<int> AddDatabase(DatabaseEntity database)
        {
            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();


                string sql =
                    "Insert into `databases` (UserId,Server,Username,Password,Port,Provider,StartDate,EndDate,IsActive,IsStorageSupport,IsApiSupport,`Database`) " +
                    "Values (@UserId,@Server,@Username,@Password,@Port,@Provider,@StartDate,@EndDate,@IsActive,@IsStorageSupport,@IsApiSupport,@Database); " +
                    "SELECT LAST_INSERT_ID();";


                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {

                    cmd.AddParameter("UserId", database.UserId);
                    cmd.AddParameter("Server", database.Server);
                    cmd.AddParameter("Username", database.Username);
                    cmd.AddParameter("Password", database.Password);
                    cmd.AddParameter("Port", database.Port);
                    cmd.AddParameter("Provider", database.Provider);
                    cmd.AddParameter("StartDate", database.StartDate);
                    cmd.AddParameter("EndDate", database.EndDate);
                    cmd.AddParameter("IsActive", database.IsActive);
                    cmd.AddParameter("IsStorageSupport", database.IsStorageSupport);
                    cmd.AddParameter("IsApiSupport", database.IsApiSupport);
                    cmd.AddParameter("Database", database.Database);

                    var id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }




            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }


        public async Task UpdateDatabase(DatabaseEntity database)
        {
            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql =
                    "Update `databases` Set UserId = @UserId,Server = @Server,Username = @Username,Password = @Password,Port = @Port,Provider = @Provider," +
                    "StartDate = @StartDate,EndDate = @EndDate,IsActive = @IsActive,IsStorageSupport = @IsStorageSupport,IsApiSupport = @IsApiSupport ,`Database`=@Database where DatabaseId = @DatabaseId";
                var cmd = conn.CreateCommand(sql);

                using (cmd)
                {
                    cmd.AddParameter("UserId", database.UserId);
                    cmd.AddParameter("Server", database.Server);
                    cmd.AddParameter("Username", database.Username);
                    cmd.AddParameter("Password", database.Password);
                    cmd.AddParameter("Port", database.Port);
                    cmd.AddParameter("Provider", database.Provider);
                    cmd.AddParameter("StartDate", database.StartDate);
                    cmd.AddParameter("EndDate", database.EndDate);
                    cmd.AddParameter("IsActive", database.IsActive);
                    cmd.AddParameter("IsStorageSupport", database.IsStorageSupport);
                    cmd.AddParameter("IsApiSupport", database.IsApiSupport);
                    cmd.AddParameter("DatabaseId", database.DatabaseId);
                    cmd.AddParameter("Database", database.Database);
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }


        }

        public async Task DeleteDatabase(DatabaseEntity database)
        {

            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql = "Delete from `databases` where DatabaseId = @DatabaseId";



                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {
                    cmd.AddParameter("DatabaseId", database.DatabaseId);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }

        }

        public async Task<List<DatabaseEntity>> GetByUser(int userId)
        {
            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql = "Select * from `databases` where UserId = @UserId";
                var cmd = conn.CreateCommand(sql);

                using (cmd)
                {
                    cmd.AddParameter("UserId", userId);

                    var sqlReader = cmd.ExecuteReader();
                    if (!sqlReader.Read())
                    {
                        return null;
                    }

                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    List<DatabaseEntity> databases = new List<DatabaseEntity>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var dataRow = dt.Rows[i];
                        var databaseEntity = ProcessDatabaseEntity(dataRow);
                        databases.Add(databaseEntity);
                    }

                    return databases;
                }


            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }

        }

        public async Task<List<DatabaseEntity>> GetByUser(string username)
        {
            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql = "Select * from `databases` inner join users on databases.UserId = users.UserId Where users.Username = @Username";
                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {

                    cmd.AddParameter("Username", username);


                    var sqlReader = cmd.ExecuteReader();
                    if (!sqlReader.Read())
                    {
                        return null;
                    }

                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    List<DatabaseEntity> databases = new List<DatabaseEntity>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var dataRow = dt.Rows[i];
                        var databaseEntity = ProcessDatabaseEntity(dataRow);
                        databases.Add(databaseEntity);
                    }

                    return databases;
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);

            }

        }

        private static DatabaseEntity ProcessDatabaseEntity(DataRow dataRow)
        {
            var databaseEntity = new DatabaseEntity
            {
                Username = dataRow["Username"] as string,
                Provider = dataRow["Provider"] as string,
                Port = dataRow["Port"] as string,
                Password = dataRow["Password"] as string,
                StartDate = Convert.ToDateTime(dataRow["StartDate"]),
                Database = dataRow["Database"] as string,
                IsActive = Convert.ToBoolean(dataRow["IsActive"]),
                IsApiSupport = Convert.ToBoolean(dataRow["IsApiSupport"]),
                IsStorageSupport = Convert.ToBoolean(dataRow["IsStorageSupport"]),
                EndDate = Convert.ToDateTime(dataRow["EndDate"]),
                DatabaseId = Convert.ToInt32(dataRow["DatabaseId"]),
                Server = dataRow["Server"] as string,
                UserId = Convert.ToInt32(dataRow["UserId"])
            };
            return databaseEntity;
        }

        public async Task<DatabaseEntity> GetById(int databaseId)
        {
            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql = "Select * from `databases` where DatabaseId = @DatabaseId";
                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {
                    cmd.AddParameter("DatabaseId", databaseId);

                    var sqlReader = cmd.ExecuteReader();
                    if (!sqlReader.Read())
                    {
                        return null;
                    }

                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    DatabaseEntity databaseEntity;
                    var dataRow = dt.Rows[0];
                    databaseEntity = ProcessDatabaseEntity(dataRow);
                    return databaseEntity;
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task<List<DatabaseEntity>> GetAll()
        {
            try
            {
                using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                string sql = "Select * from `databases`";
                //  var GetAllDatabase = (List<DatabaseEntity>)await cmd.ExecuteScalarAsync();

                var cmd = conn.CreateCommand(sql);
                using (cmd)
                {
                    var sqlReader = cmd.ExecuteReader();
                    if (!sqlReader.Read())
                    {
                        return null;
                    }

                    DataTable dt = new DataTable();
                    dt.Load(sqlReader);
                    List<DatabaseEntity> databases = new List<DatabaseEntity>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var dataRow = dt.Rows[i];
                        var databaseEntity = ProcessDatabaseEntity(dataRow);
                        databases.Add(databaseEntity);
                    }

                    return databases;
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public Task SuspendDatabase(int databaseId)
        {
            throw new NotImplementedException();
        }

        public Task RecoverDatabase(int databaseId)
        {
            throw new NotImplementedException();
        }
    }
}