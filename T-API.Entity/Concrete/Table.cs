using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace T_API.Entity.Concrete
{
    public class Table
    {
        public Table()
        {
            ForeignKeys = new ObservableCollection<ForeignKey>();
            Indices=new ObservableCollection<Index>();
            Keys=new ObservableCollection<Key>();
            Columns=new ObservableCollection<Column>();

        }
        public string TableName { get; set; }
        public ObservableCollection<ForeignKey> ForeignKeys { get; set; }
        public ObservableCollection<Index> Indices { get; set; }
        public ObservableCollection<Key> Keys { get; set; }
        public ObservableCollection<Column> Columns { get; set; }

    }
}