using System.Collections.ObjectModel;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;

namespace T_API.Core.DTO.Table
{
    public class UpdateTableDto
    {
        public string TableName { get; set; }
        public ObservableCollection<UpdateForeignKeyDto> ForeignKeys { get; set; }
        public ObservableCollection<UpdateIndexDto> Indices { get; set; }
        public ObservableCollection<UpdateKeyDto> Keys { get; set; }
        public ObservableCollection<UpdateColumnDto> Columns { get; set; }
    }
}