using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using T_API.Core.DTO.DatabasePackage;
using T_API.Core.DTO.Table;

namespace T_API.UI.Models.Database
{
    public class CreateServiceViewModel
    {
        public int UserId { get; set; }
        public string Provider { get; set; }
        [Required] public int PackageId { get; set; }
        public List<DetailDatabasePackageDto> Packages { get; set; }
        public List<string> Providers { get; set; }
        public int MonthCount { get; set; }
    }
}