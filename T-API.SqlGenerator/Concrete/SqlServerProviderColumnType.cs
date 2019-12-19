namespace T_API.SqlGenerator.Concrete
{
    public class SqlServerProviderColumnType:ProviderColumnType
    {

        #region Exact Numerics
        public static readonly string INT = "int";
        public static readonly string TINYINT = "tinyint";
        public static readonly string SMALLINT = "smallint";
        public static readonly string BIGINT = "bigint";
        public static readonly string DECIMAL = "decimal";
        public static readonly string MONEY = "money";
        public static readonly string SMALLMONEY = "smallmoney";
        public static readonly string NUMERIC = "numeric";
        #endregion

        #region Approximate Numerics
        public static readonly string FLOAT = "float";
        public static readonly string REAL = "real";
        #endregion

        #region date and datetime
        public static readonly string DATE = "date";
        public static readonly string DATETIME = "datetime";
        public static readonly string DATETIME2 = "datetime2";
        public static readonly string SMALLDATETIME = "smalldatetime";
        public static readonly string TIME = "time";
        #endregion

        #region strings
        public static readonly string CHAR = "char";
        public static readonly string VARCHAR = "varchar";
        public static readonly string TEXT = "text";
        public static readonly string NCHAR = "nchar";
        public static readonly string NVARCHAR = "nvarchar";
        public static readonly string NTEXT = "ntext";
        public static readonly string IMAGE = "image";
        #endregion

        #region other
        public static readonly string UNIQUEIDENTIFIER = "uniqueidentifier";
        public static readonly string XML = "xml";
        #endregion

    }
}