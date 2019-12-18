namespace T_API.SqlGenerator
{
    public class SqlServerColumnType:IColumnType
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

    }
}