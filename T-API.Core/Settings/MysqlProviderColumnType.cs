using T_API.Entity.Abstract;

namespace T_API.Core.Settings
{
   public class MysqlProviderColumnType:ProviderColumnType
    {
        #region numerics
        public static readonly string INT = "int";
        public static readonly string TINYINT = "tinyint";
        public static readonly string SMALLINT = "smallint";
        public static readonly string BIGINT = "bigint";
        public static readonly string BOOL = "bool";
        public static readonly string BIT = "bit";
        public static readonly string FLOAT = "float";
        public static readonly string DECIMAL = "decimal";
        public static readonly string DOUBLE = "double";

        #endregion

        #region strings
        public static readonly string CHAR = "char";
        public static readonly string VARCHAR = "varchar";
        public static readonly string BINARY = "binary";
        public static readonly string TEXT = "text";
        public static readonly string TINYTEXT = "tinytext";
        public static readonly string MEDIUMTEXT = "mediumtext";
        public static readonly string LONGTEXT = "longtext";
        public static readonly string BLOB = "blob";
        public static readonly string MEDIUMBLOB = "mediumblob";
        public static readonly string LONGBLOB = "longblob";

        public static readonly string ENUM = "enum";

        #endregion

        #region date
        public static readonly string DATE = "date";
        public static readonly string DATETIME = "datetime";
        public static readonly string TIMESTAMP = "timestamp";
        public static readonly string TIME = "time";
        public static readonly string YEAR = "year";
        #endregion


    }
}
