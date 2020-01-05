using System;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using MySql.Data.MySqlClient;

namespace T_API.Core.Exception
{
    public  class ExceptionHandler
    {
        public static System.Exception HandleException(System.Exception e)
        {
            switch (e)
            {
                case MySqlException _:
                    return MySqlExceptionHandler.HandleMySqlException(e);
                case SqlException _:
                    return SqlServerExceptionHandler.HandleMssqlException(e);
                case NullReferenceException _:
                case ArgumentException _:
                case ArrayTypeMismatchException _:
                case ArithmeticException _:
                case IndexOutOfRangeException _:
                case InvalidCastException _:
                case ValidationException _:
                    return e;
                default:
                    return e;
            }
        }
    }
}