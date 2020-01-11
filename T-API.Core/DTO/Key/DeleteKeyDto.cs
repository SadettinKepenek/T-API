namespace T_API.Core.DTO.Key
{
    public class DeleteKeyDto
    {
        public string KeyName { get; set; }
        public int DatabaseId { get; set; }
        public string Provider { get; set; }
        public string TableName { get; set; }
    }
}