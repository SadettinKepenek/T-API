using System.Collections.Generic;
using T_API.Core.DTO.EndPoint;
using T_API.Core.DTO.Table;

namespace T_API.BLL.Abstract
{
    public interface IEndPointService
    {
        List<EndPointModel> GetEndPoints(DetailTableDto table, int userId, int databaseId);
    }
}