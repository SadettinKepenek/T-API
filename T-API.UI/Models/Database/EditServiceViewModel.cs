using System;
using System.ComponentModel.DataAnnotations;

namespace T_API.UI.Models.Database
{
    public class EditServiceViewModel
    {
 

        public EditServiceViewModel()
        {
            
        }

        public int UserId { get; set; }
        public int DatabaseId { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }

        public bool IsActive { get; set; }
        public bool IsStorageSupport { get; set; }
        public bool IsApiSupport { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}