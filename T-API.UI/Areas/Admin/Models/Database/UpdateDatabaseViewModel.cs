using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T_API.UI.Areas.Admin.Models.Database
{
    public class UpdateDatabaseViewModel
    {
        public int DatabaseId { get; set; }
        public int UserId { get; set; }
        [Required]
        public string Server { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Database { get; set; }
        [Required]
        public string Port { get; set; }
        [Required]
        public string Provider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsStorageSupport { get; set; }
        public bool IsApiSupport { get; set; }
    }
}
