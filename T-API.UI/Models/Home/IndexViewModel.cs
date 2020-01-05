using System.Collections.Generic;
using T_API.Core.DTO.Database;
using T_API.Core.DTO.DatabasePackage;
using T_API.Core.DTO.User;

namespace T_API.UI.Models.Home
{
    public class IndexViewModel
    {
        public List<DetailDatabasePackageDto> Packages { get; set; }
        public DetailUserDto User { get; internal set; }
        public List<ListDatabaseDto> Databases { get; set; }
    }
}