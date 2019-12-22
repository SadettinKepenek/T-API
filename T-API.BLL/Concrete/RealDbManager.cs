using System;
using System.Threading.Tasks;
using T_API.BLL.Abstract;
using T_API.Core.Exception;
using T_API.Entity.Concrete;

namespace T_API.BLL.Concrete
{
    public class RealDbManager:IRealDbService
    {
        private ISqlCodeGeneratorFactory _codeGeneratorFactory;
        //Factory Design Pattern

        public RealDbManager(ISqlCodeGeneratorFactory codeGeneratorFactory)
        {
            _codeGeneratorFactory = codeGeneratorFactory;
        }

        public async Task CreateDatabaseOnRemote(Database database)
        {
            try
            {
                if (database.DatabaseEntity.Provider.Equals("MySql"))
                {
                    var generator = _codeGeneratorFactory.CreateGenerator(database.DatabaseEntity.Provider);
                    
                }
            }
            catch (Exception e)
            {
                throw ExceptionHandler.HandleException(e);
            }

        }

        public async Task CreateTableOnRemote(Database database)
        {
            throw new System.NotImplementedException();
        }

        public async Task CreateColumnOnRemote(Database database)
        {
            throw new System.NotImplementedException();
        }
    }
}