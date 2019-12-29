namespace T_API.Core.DTO.Column
{
    public class UpdateColumnDto
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }

        public string DataType { get; set; }
        public int DataLength { get; set; }
        public bool NotNull { get; set; }
        public bool AutoInc { get; set; }
        public bool Unique { get; set; }
        public bool PrimaryKey { get; set; }
        public object DefaultValue { get; set; }

        public bool HasLength { get; set; }
        public string Provider { get; set; }

    }
}