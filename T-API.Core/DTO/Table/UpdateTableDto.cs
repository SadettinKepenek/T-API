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
        public ObservableCollection<ListForeignKeyDto> ForeignKeys { get; set; }
        public ObservableCollection<ListIndexDto> Indices { get; set; }
        public ObservableCollection<ListKeyDto> Keys { get; set; }
        public ObservableCollection<ListColumnDto> Columns { get; set; }
    }
}