namespace T_API.Core.DTO.Index
{
    public class DetailIndexDto
    {
        public string IndexName { get; set; }
        public string IndexColumn { get; set; }
        public string IndexOrder { get; set; }
        public bool IsUnique { get; set; }

        public string TableName { get; set; }
    }
}