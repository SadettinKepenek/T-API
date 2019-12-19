namespace T_API.Entity.Concrete
{
    public class Index
    {
        public string IndexName { get; set; }
        public string IndexColumn { get; set; }
        public string IndexOrder { get; set; }
        public bool IsUnique { get; set; }

        public Column Column { get; set; }
    }
}