using T_API.Entity.Abstract;

namespace T_API.Entity.Concrete
{
    public class Column
    {
        public string ColumnName { get; set; }
        public ProviderColumnType DataType { get; set; }
        public int DataLength { get; set; }
        public bool NotNull { get; set; }
        public bool AutoInc { get; set; }
        public bool Unique { get; set; }
        public bool PrimaryKey { get; set; }
        public object DefaultValue { get; set; }

        public Table Table { get; set; }


     
    }
}