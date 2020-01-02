using System.Collections.Generic;

namespace T_API.Core.DTO.EndPoint
{
    public class EndPointModel
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public string Example { get; set; }
        public List<ParamModel> Params { get; set; }
    }
}