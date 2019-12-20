using System.Collections.Generic;
using System.Threading.Tasks;
using T_API.Core.DTO.Database;

namespace T_API.BLL.Abstract
{
    public interface IDatabaseService
    {
        Task<List<ListDatabaseDto>> GetAll();
        Task<List<ListDatabaseDto>> GetByUser(int userId);
        Task<DetailDatabaseDto> GetById(int databaseId);
        Task<int> AddDatabase(AddDatabaseDto dto);
        Task UpdateDatabase(UpdateDatabaseDto dto);
        Task DeleteDatabase(DeleteDatabaseDto dto);
    }
}