using System.Data;
using System.Threading.Tasks;
using T_API.Core.DAL.Concrete;

namespace T_API.BLL.Abstract
{
    public interface IDataService
    {
        Task<string> Get(string tableName, DbInformation dbInformation);
    }
}