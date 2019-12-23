using System;
using System.Reflection;
using T_API.Core.DAL.Abstract;
using T_API.DAL.Abstract;

namespace T_API.DAL.Concrete
{
    public class RealDbRepositoryFactory : IRealDbRepositoryFactory
    {
        // TODO CreateConnection dynamic tipte bir connection döndürüyor bunun kontrol edilmesi gerekli

        private MySqlRealDbRepository _mySqlRealDbRepository;

        public MySqlRealDbRepository MySqlRealDbRepository
        {
            get
            {
                if (_mySqlRealDbRepository == null)
                {
                    var sqlRealDbRepository = new MySqlRealDbRepository();
                    _mySqlRealDbRepository = sqlRealDbRepository;
                    return sqlRealDbRepository;
                }

                return _mySqlRealDbRepository;
            }
            set { _mySqlRealDbRepository = value; }
        }

        private MssqlRealDbRepository _mssqlRealDbRepository;

        public MssqlRealDbRepository MssqlRealDbRepository
        {
            get
            {
                if (_mssqlRealDbRepository == null)
                {
                    var sqlDbRepository = new MssqlRealDbRepository();
                    _mssqlRealDbRepository = sqlDbRepository;
                    return _mssqlRealDbRepository;
                }

                return _mssqlRealDbRepository;
            }
            set { _mssqlRealDbRepository = value; }
        }

        private IServiceProvider _serviceProvider;

        public RealDbRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public IRealDbRepository CreateRepository(string provider)
        {
            if (provider.Equals("MySql"))
            {
                return MySqlRealDbRepository;
            }
            else if (provider.Equals("MsSql"))
            {
                return MssqlRealDbRepository;
            }
            else
            {
                throw new AmbiguousMatchException("Provider desteklenmiyor");
            }
        }
    }
}