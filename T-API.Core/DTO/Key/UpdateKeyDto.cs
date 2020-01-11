namespace T_API.Core.DTO.Key
{
    public class UpdateKeyDto
    {
        public int DatabaseId { get; set; }

        public string KeyName { get; set; }
        public string KeyColumn { get; set; }
        public bool IsPrimary { get; set; }
        public DetailKeyDto OldKey { get; set; }
        public string TableName { get; set; }
    }
}