using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using T_API.Core.DTO.Column;
using T_API.Core.DTO.ForeignKey;
using T_API.Core.DTO.Index;
using T_API.Core.DTO.Key;

namespace T_API.UI.Models.Database
{
    public class AddTableViewModel
    {
        [Required]
        public string TableName { get; set; }
        [Required]
        public string DatabaseName { get; set; }
        [Required]
        public int DatabaseId { get; set; }
        [Required]
        public string Provider { get; set; }
        [Required]
        public List<AddColumnDto> Columns { get; set; }
        public List<AddForeignKeyDto> ForeignKeys { get; set; }
        public List<AddIndexDto> Indices { get; set; }
        public List<AddKeyDto> Keys { get; set; }
    }
}