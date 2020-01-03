using System;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using T_API.BLL.Abstract;
using T_API.Core.DAL.Concrete;
using T_API.Core.Exception;
using T_API.DAL.Abstract;
using T_API.DAL.Concrete;

namespace T_API.BLL.Concrete
{
    public class DataManager : IDataService
    {
        private IRealDbRepositoryFactory _realDbRepositoryFactory;

        public DataManager(IRealDbRepositoryFactory realDbRepositoryFactory)
        {
            _realDbRepositoryFactory = realDbRepositoryFactory;
        }

        public async Task<string> Get(string tableName, DbInformation dbInformation)
        {
            try
            {

                if (_realDbRepositoryFactory.CreateRepository(dbInformation.Provider) is MySqlRealDbRepository realDbRepository)
                {
                    using TransactionScope scope = new TransactionScope();

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine($"USE {dbInformation.DatabaseName};\n");
                    stringBuilder.AppendLine($"Select * from {tableName}");
                    var dt=await realDbRepository.Get(stringBuilder.ToString(), dbInformation);
                    string result = JsonConvert.SerializeObject(dt);
                    scope.Complete();
                    return result;
                }
                else
                {
                    throw new NullReferenceException("Db Repository Referansına Ulaşlamadı");
                }

            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }
    }
}