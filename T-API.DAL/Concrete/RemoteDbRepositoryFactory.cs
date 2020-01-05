using System;
using T_API.DAL.Abstract;

namespace T_API.DAL.Concrete
{
    public class RemoteDbRepositoryFactory
    {
        public static IRealDbRepository CreateRepository(string provider)
        {
            if (provider.Equals("MySql"))
            {
                return new MySqlRemoteDbRepository();
            }

            throw new Exception("Provider Desteklenmiyor.");
        }
    }
}