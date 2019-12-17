using System.Data;
using System.Threading.Tasks;
using T_API.Core.DAL.Concrete;

namespace T_API.Core.DAL.Abstract
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection(DbInformation dbInformation);
    }
}