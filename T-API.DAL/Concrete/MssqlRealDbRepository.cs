using System.Threading.Tasks;
using T_API.Core.DAL.Abstract;
using T_API.DAL.Abstract;

namespace T_API.DAL.Concrete
{
    public class MssqlRealDbRepository: IRealDbRepository
    {
        // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli
        private IDbConnectionFactory _dbConnectionFactory;

        public MssqlRealDbRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public Task CreateDatabaseOnRemote(string query)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateTableOnRemote(string query)
        {
            throw new System.NotImplementedException();
        }

        public Task CreateColumnOnRemote(string query)
        {
            throw new System.NotImplementedException();
        }
    }
}