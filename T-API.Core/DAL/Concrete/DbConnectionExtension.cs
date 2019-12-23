using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using MySql.Data.MySqlClient;

namespace T_API.Core.DAL.Concrete
{
    public static class DbConnectionExtension
    {

        

        private static SqlCommand CreateSqlCommand(string query, IDbConnection conn)
        {
            return new SqlCommand(query,conn as SqlConnection);
        }
        private static MySqlCommand CreateMySqlCommand(string query, IDbConnection conn)
        {
            return new MySqlCommand(query, conn as MySqlConnection);
        }



        public static IDbCommand CreateCommand(this IDbConnection conn, string query)
        {
            if (conn is MySqlConnection)
            {
                return CreateMySqlCommand(query,conn);
            }
            else if (conn is SqlConnection)
            {
                return CreateSqlCommand(query, conn);
            }
            else
            {
                throw new ArgumentException("Connection türü desteklenmiyor.");
            }
        }

       



    }
}