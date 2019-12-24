using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using MySql.Data.MySqlClient;
using T_API.Core.DAL.Concrete;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;

namespace T_API.DAL.Concrete
{
    public class MySqlRealDbRepository : IRealDbRepository
    {
        // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli


        public MySqlRealDbRepository()
        {
        }


        public async Task CreateDatabaseOnRemote(string query)
        {

            using (var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
            {


                var cmd = conn.CreateCommand(query);

                using (cmd)
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw ExceptionHandler.HandleException(ex);

                    }
                }

            }



        }

        public async Task CreateTableOnRemote(string query)
        {
            using (var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
            {


                var cmd = conn.CreateCommand(query);

                using (cmd)
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ex.Message);
                        throw ExceptionHandler.HandleException(ex);

                    }
                }

            }

        }

        [Obsolete("Method daha eklenmedi lütfen daha sonra tekrar bakın.")]
        public Task CreateColumnOnRemote(string query)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateIndexOnRemote(string query)
        {
            throw new NotImplementedException();
        }

        public Task CreateForeignKeyOnRemote(string query)
        {
            throw new NotImplementedException();
        }

        public Task CreateKeyOnRemote(string query)
        {
            throw new NotImplementedException();
        }
    }
}