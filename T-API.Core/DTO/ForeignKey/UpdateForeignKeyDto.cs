namespace T_API.Core.DTO.ForeignKey
{
    public class UpdateForeignKeyDto
    {
        public string ForeignKeyName { get; set; }
        public string TargetTable { get; set; }
        public string SourceTable { get; set; }
        public string TargetColumn { get; set; }
        public string SourceColumn { get; set; }
        public string OnUpdateAction { get; set; }
        public string OnDeleteAction { get; set; }
        public DetailForeignKeyDto OldForeignKey { get; set; }
        public int DatabaseId { get; set; }

    }
}