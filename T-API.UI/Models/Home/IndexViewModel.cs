using System.Collections.Generic;
using T_API.Core.DTO.DatabasePackage;

namespace T_API.UI.Models.Home
{
    public class IndexViewModel
    {
        public List<DetailDatabasePackageDto> Packages { get; set; }
    }
}