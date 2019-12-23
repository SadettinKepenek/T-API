using System.Data;
using T_API.Core.DAL.Abstract;

namespace T_API.Core.DAL.Concrete
{
    public class UnitOfWorkFactory
    {
        private static AdoNetUnitOfWork Instance { get; set; }

        private UnitOfWorkFactory()
        {

        }
        public static IUnitOfWork Create(DbInformation information)
        {
            var adoNetUnitOfWork = new AdoNetUnitOfWork(information, true);
            Instance = adoNetUnitOfWork;
            return adoNetUnitOfWork;
        }

        public static IUnitOfWork GetInstance()
        {
            return Instance;
        }

    }
}