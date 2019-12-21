using System;
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
                "Insert into `databases` (UserId,Server,Username,Password,Port,Provider,StartDate,EndDate,IsActive,IsStorageSupport,IsApiSupport,`Database`) " + 
                "Values (@UserId,@Server,@Username,@Password,@Port,@Provider,@StartDate,@EndDate,@IsActive,@IsStorageSupport,@IsApiSupport,@Database); " +
                "SELECT LAST_INSERT_ID();";
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
            cmd.Parameters.AddWithValue("Database", database.Database);

            var id = Convert.ToInt32(await cmd.ExecuteScalarAsync()); 
            return id;
        }

        public async Task UpdateDatabase(DatabaseEntity database)
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if(conn.State == ConnectionState.Broken||conn.State == ConnectionState.Closed) conn.Open();

            string sql =
                "Update `databases` Set UserId = @UserId,Server = @Server,Username = @Username,Password = @Password,Port = @Port,Provider = @Provider," +
                "StartDate = @StartDate,EndDate = @EndDate,IsActive = @IsActive,IsStorageSupport = @IsStorageSupport,IsApiSupport = @IsApiSupport ,`Database`=@Database where DatabaseId = @DatabaseId";
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
            cmd.Parameters.AddWithValue("Database", database.Database);


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

            var sqlReader = cmd.ExecuteReader();
            if (!sqlReader.HasRows)
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
                var dataRow = dt.Rows[i];
                var databaseEntity = ProcessDatabaseEntity(dataRow);
                databases.Add(databaseEntity);
            }

            return databases;

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
                UserId = Convert.ToInt32(dataRow["UserId"]),
            };
            return databaseEntity;
        }

        public async Task<DatabaseEntity> GetById(int databaseId)
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

            string sql = "Select * from `databases` where DatabaseId = @DatabaseId";
            using var cmd = new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("DatabaseId", databaseId);

            var sqlReader = cmd.ExecuteReader();
            if (!sqlReader.HasRows)
            {
                return null;
            }

            DataTable dt = new DataTable();
            dt.Load(sqlReader);
            var databaseEntity = new DatabaseEntity();

            var dataRow = dt.Rows[0];
            databaseEntity = ProcessDatabaseEntity(dataRow);
            return databaseEntity;

        }

        public async Task<List<DatabaseEntity>> GetAll()
        {
            using var conn = _dbConnectionFactory.CreateConnection(ConfigurationSettings.DbInformation);
            if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

            string sql = "Select * from `databases`";
            using var cmd = new MySqlCommand("sql",conn as MySqlConnection);
            cmd.CommandText = sql;
            var GetAllDatabase = (List<DatabaseEntity>) await cmd.ExecuteScalarAsync();

            var sqlReader = cmd.ExecuteReader();
            if (!sqlReader.HasRows)
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