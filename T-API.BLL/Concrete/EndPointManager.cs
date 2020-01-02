using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using T_API.BLL.Abstract;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.EndPoint;
using T_API.Core.DTO.Table;

namespace T_API.BLL.Concrete
{
    public class EndPointManager : IEndPointService
    {


        public List<EndPointModel> GetEndPoints(DetailTableDto table, int userId, int databaseId)
        {

            List<EndPointModel> endPointModels = new List<EndPointModel>();
            endPointModels.Add(new EndPointModel
            {
                Type = "GET",
                Url = $"api/RealDatabase/Get?UserId={userId}&DatabaseId={databaseId}&Table={table.TableName}",
            });
            endPointModels.Add(GeneratePostEndpoint(table, userId, databaseId));

            return endPointModels;
        }

        private EndPointModel GeneratePostEndpoint(DetailTableDto table, int userId, int databaseId)
        {
            EndPointModel model = new EndPointModel();
            model.Url = $"api/RealDatabase/Add?UserId={userId}&DatabaseId={databaseId}&Table={table.TableName}";
            model.Type = "POST";

            model.Params = new List<ParamModel>();
            model.Params.Add(new ParamModel()
            {
                Example = "Kullanıcı Adınız",
                ParamName = "UserId",
                ParamType = typeof(int).ToString()
            });
            model.Params.Add(new ParamModel()
            {
                Example = "Servis Numaranız",
                ParamName = "DatabaseId",
                ParamType = typeof(int).ToString()
            });
            model.Params.Add(new ParamModel()
            {
                Example = "Veri eklemek istediğiniz tablo ismi",
                ParamName = "Table",
                ParamType = typeof(string).ToString()
            });

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\nPOST REQUEST JSON DATA EXAMPLE\n\n");
            foreach (ListColumnDto column in table.Columns)
            {
                if (!column.PrimaryKey)
                {
                    stringBuilder.AppendLine($"\"{column.ColumnName}\":{column.DataType} DATA");
                }
            }

            model.Example = stringBuilder.ToString();
            return model;
        }
    }
}