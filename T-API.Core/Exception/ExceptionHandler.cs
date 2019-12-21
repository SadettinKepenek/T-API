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
            if (e is MySqlException)
            {
                return MySqlExceptionHandler.HandleMySqlException(e);
            }

            if (e is SqlException)
            {
                return SqlServerExceptionHandler.HandleMssqlException(e);
            }

            if (e is NullReferenceException)
            {
                return e;
            }

            if (e is ArgumentException)
            {
                return e;
            }

            if (e is ArrayTypeMismatchException)
            {
                return e;

            }

            if (e is ArithmeticException)
            {
                return e;

            }

            if (e is IndexOutOfRangeException)
            {
                return e;

            }

            if (e is InvalidCastException)
            {
                return e;

            }

            if (e is ValidationException)
            {
                return e;
            }
            return e;
        }
    }
}