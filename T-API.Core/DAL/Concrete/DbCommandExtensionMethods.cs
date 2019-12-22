using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace T_API.Core.DAL.Concrete
{
    public static class DbCommandExtensionMethods
    {
        public static void AddParameter(this IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}