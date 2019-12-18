namespace T_API.Entity.Concrete
{
    public class ForeignKey
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