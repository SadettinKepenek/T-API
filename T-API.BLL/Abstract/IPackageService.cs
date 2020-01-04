using System.Collections.Generic;
using System.Threading.Tasks;
using T_API.Core.DTO.DatabasePackage;

namespace T_API.BLL.Abstract
{
    public interface IPackageService
    {
        Task<List<DetailDatabasePackageDto>> Get();
        Task<DetailDatabasePackageDto> GetById(int id);
        Task<DetailDatabasePackageDto> GetByName(string id);
        Task Add(AddDatabasePackageDto package);
        Task Update(UpdateDatabasePackageDto package);
        Task Delete(DeleteDatabasePackageDto package);
    }
}