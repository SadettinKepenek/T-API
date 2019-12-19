namespace T_API.Entity.Concrete
{
    public class Key
    {
        public string KeyName { get; set; }
        public string KeyColumn { get; set; }
        public bool IsPrimary { get; set; }

        public Column Column { get; set; }
    }
}