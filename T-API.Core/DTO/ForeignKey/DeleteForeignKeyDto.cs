namespace T_API.Core.DTO.ForeignKey
{
    public class DeleteForeignKeyDto
    {
        public string ForeignKeyName { get; set; }
        public string SourceTable { get; set; }
        public int DatabaseId { get; set; }


    }
}