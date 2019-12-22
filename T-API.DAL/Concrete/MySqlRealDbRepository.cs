using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using T_API.Core.DAL.Abstract;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;

namespace T_API.DAL.Concrete
{
    public class MySqlRealDbRepository:IRealDbRepository
    {
        // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli

        private IDbConnectionFactory _dbConnectionFactory;

        public MySqlRealDbRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }


        public async Task CreateDatabaseOnRemote(string query)
        {
            try
            {
                using (var conn=_dbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
                {
                    if (conn.State==ConnectionState.Broken || conn.State==ConnectionState.Closed) conn.Open();

                    await using (var cmd=new MySqlCommand(query,conn as MySqlConnection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }

                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }
        }

        [Obsolete("Method daha eklenmedi lütfen daha sonra tekrar bakın.")]
        public Task CreateTableOnRemote(string query)
        {
            throw new System.NotImplementedException();
        }

        [Obsolete("Method daha eklenmedi lütfen daha sonra tekrar bakın.")]
        public Task CreateColumnOnRemote(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}