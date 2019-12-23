using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using T_API.Core.DAL.Abstract;
using IDbConnection = System.Data.IDbConnection;

namespace T_API.Core.DAL.Concrete
{
    public class DbConnectionFactory
    {

        public static IDbConnection CreateConnection(DbInformation information)
        {
            string connectionString;
            if (information.Provider.Equals("SqlServer"))
            {
                if (String.IsNullOrEmpty(information.Port))
                    information.Port = "3306";
                connectionString = $"Data Source={information.Server};" +
                                          $"Initial Catalog={information.Database};" +
                                          $"User id={information.Username};" +
                                          $"Password={information.Password}";
                var conn = new SqlConnection(connectionString);

                conn.Open();
                return conn;
            }

            if (information.Provider.Equals("MySql"))
            {
                if (String.IsNullOrEmpty(information.Port))
                    information.Port = "1443";
                connectionString = $"Server={information.Server};" +
                                   $"Initial Catalog={information.Database};" +
                                   $"Uid={information.Username};" +
                                   $"Pwd={information.Password};";
                var mySqlConnection = new MySqlConnection(connectionString);
                mySqlConnection.Open();
                return mySqlConnection;
            }

            throw new InvalidEnumArgumentException("Sql Provider Is Not Selected");


        }

    }
}