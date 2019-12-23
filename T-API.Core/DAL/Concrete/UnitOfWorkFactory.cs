using System.Data;
using T_API.Core.DAL.Abstract;

namespace T_API.Core.DAL.Concrete
{
    public static class UnitOfWorkFactory
    {
        public static IUnitOfWork Create(IDbConnection connection)
        {
            return new AdoNetUnitOfWork(connection,true);
        }
    }
}