namespace T_API.Core.DTO.ForeignKey
{
    public class ListForeignKeyDto
    {
        public string ForeignKeyName { get; set; }
        public string TargetTable { get; set; }
        public string SourceTable { get; set; }
        public string TargetColumn { get; set; }
        public string SourceColumn { get; set; }
        public string OnUpdateAction { get; set; }
        public string OnDeleteAction { get; set; }
    }
}