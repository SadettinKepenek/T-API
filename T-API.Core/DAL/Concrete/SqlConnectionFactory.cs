using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using T_API.Core.DAL.Abstract;
using IDbConnection = System.Data.IDbConnection;

namespace T_API.Core.DAL.Concrete
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {

        public async Task<IDbConnection> CreateConnection(DbInformation information)
        {
            string connectionString;
            if (information.Provider.Equals("SqlServer"))
            {
                if (String.IsNullOrEmpty(information.Port))
                    information.Port = "3306";
                connectionString = $"Data Source={information.Server};" +
                                          $"Initial Catalog={information.Database};" +
                                          $"User id={information.Username};" +
                                          $"Password={information.Password}" +
                                          $"Port={information.Port};";
            }
            else if (information.Provider.Equals("MySql"))
            {
                if (String.IsNullOrEmpty(information.Port))
                    information.Port = "1443";
                connectionString = $"Server={information.Server};" +
                                          $"Initial Catalog={information.Database};" +
                                          $"Uid={information.Username};" +
                                          $"Pwd={information.Password};" + 
                                          $"Port={information.Port};";
            }
            else
            {
                throw new InvalidEnumArgumentException("Sql Provider Is Not Selected");
            }

            var conn = new SqlConnection(connectionString);
            try
            {
                await conn.OpenAsync();
                return conn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}