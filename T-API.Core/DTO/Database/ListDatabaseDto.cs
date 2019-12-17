using System;
using T_API.Core.DAL.Concrete;

namespace T_API.Core.DTO.Database
{
    public class ListDatabaseDto
    {
        public int DatabaseId { get; set; }
        public int UserId { get; set; }
        public string UserFirstname { get; set; }
        public string UserLastname { get; set; }
        public string Server { get; set; }
        public string Provider { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}