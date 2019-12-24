using System.Collections.ObjectModel;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;
using T_API.Entity.Concrete;

namespace T_API.Core.DTO.Table
{
    public class AddTableDto
    {
        public string TableName { get; set; }
        public ObservableCollection<AddForeignKeyDto> ForeignKeys { get; set; }
        public ObservableCollection<AddIndexDto> Indices { get; set; }
        public ObservableCollection<AddKeyDto> Keys { get; set; }
        public ObservableCollection<AddColumnDto> Columns { get; set; }

        public string Provider { get; set; }
        public string DatabaseName { get; set; }
    }
}