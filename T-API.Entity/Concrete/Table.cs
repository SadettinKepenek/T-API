using System.Collections.Generic;

namespace T_API.Entity.Concrete
{
    public class Table
    {
        public List<ForeignKey> ForeignKeys { get; set; }
        public List<Index> Indices { get; set; }
        public List<Key> Keys { get; set; }
        public List<Column> Columns { get; set; }

    }
}