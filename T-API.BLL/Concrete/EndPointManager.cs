using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using T_API.BLL.Abstract;
using T_API.Core.DTO.EndPoint;
using T_API.Core.DTO.Table;

namespace T_API.BLL.Concrete
{
    public class EndPointManager:IEndPointService
    {
       

        public List<EndPointModel> GetEndPoints(DetailTableDto table,int userId,int databaseId)
        {

            List<EndPointModel> endPointModels=new List<EndPointModel>();
            endPointModels.Add(new EndPointModel
            {
                Type = "GET",
                Url = $"api/RealDatabase/Get?UserId={userId}&DatabaseId={databaseId}&Table={table.TableName}",
            });

            return endPointModels;
        }
    }
}