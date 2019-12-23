using System.Collections.Generic;

namespace T_API.Entity.Concrete
{
    public class Database
    {
        public List<Table> Tables { get; set; }
        public DatabaseEntity DatabaseEntity { get; set; }
    }
}