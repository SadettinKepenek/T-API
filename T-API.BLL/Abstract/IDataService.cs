using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using T_API.Core.DAL.Concrete;

namespace T_API.BLL.Abstract
{
    public interface IDataService
    {
        Task<string> Get(string tableName, DbInformation dbInformation);
        Task Add(string tableName, DbInformation dbInformation, JObject jObject);
        Task Update(string tableName, DbInformation dbInformation, JObject jObject);
    }
}