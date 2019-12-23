using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using T_API.Core.DAL.Abstract;
using T_API.Core.DAL.Concrete;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;

namespace T_API.DAL.Concrete
{
    public class MySqlRealDbRepository : IRealDbRepository
    {
        // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli

        private IUnitOfWork _unitOfWork;

        public MySqlRealDbRepository( )
        {
        }


        public async Task CreateDatabaseOnRemote(string query)
        {

            using (var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
            {
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                var cmd = _unitOfWork.CreateCommand(query);
                var transaction = conn.BeginTransaction();

                using (cmd)
                {
                    try
                    {
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ex.Message);
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                            Console.WriteLine("  Message: {0}", ex2.Message);
                            throw ExceptionHandler.HandleException(ex2);
                        }
                    }
                }

            }



            try
            {
                using (var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
                {
                    if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed) conn.Open();

                    var cmd = _unitOfWork.CreateCommand(query);
                    using (cmd)
                    {
                        var transaction = conn.BeginTransaction();
                        cmd.Transaction = transaction;

                        cmd.ExecuteNonQuery();
                        transaction.Commit();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                try
                {

                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);

                }
                throw ExceptionHandler.HandleException(ex);
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