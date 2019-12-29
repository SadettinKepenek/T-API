namespace T_API.Core.DTO.Column
{
    public class DeleteColumnDto
    {
        public string ColumnName { get; set; }
        public string TableName { get; set; }
        public string Provider { get; set; }
        public int DatabaseId { get; set; }

    }
}