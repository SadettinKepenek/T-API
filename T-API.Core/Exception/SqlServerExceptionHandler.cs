using System.Data.SqlClient;

namespace T_API.Core.Exception
{
    public class SqlServerExceptionHandler:ExceptionHandler
    {
        private const int ReferenceConstraint = 547;
        private const int CannotInsertNull = 515;
        private const int CannotInsertDuplicateKeyUniqueIndex = 2601;
        private const int CannotInsertDuplicateKeyUniqueConstraint = 2627;
        private const int ArithmeticOverflow = 8115;
        private const int StringOrBinaryDataWouldBeTruncated = 8152;
        private const int InvalidDatabase = 4060;
        private const int DatabaseLoginFailed = 18452;
        
        public static System.Exception HandleMssqlException(System.Exception e)
        {
            var ex = (SqlException)e;

            switch (ex.Number)
            {
                case ReferenceConstraint:
                    return new DatabaseException("Silinen veri için bağlı olan nesneler var.", ex.InnerException);
                case CannotInsertNull:
                    return new DatabaseException("Belirtilen veri null geçilemez.",ex.InnerException);
                case CannotInsertDuplicateKeyUniqueIndex:
                case CannotInsertDuplicateKeyUniqueConstraint:
                    return new DatabaseException("Eklenilen veri için zaten key mevcut.",ex.InnerException);
                case ArithmeticOverflow:
                    return new DatabaseException("Veritabanında hesaplama işlemlerinde hata oluştu",ex.InnerException);
                case StringOrBinaryDataWouldBeTruncated:
                    return new DatabaseException("String veya Binary veride hata oluştu",ex.InnerException);
                default:
                    return new DatabaseException("Veritabanında hata oluştu", ex.InnerException);
            }

        }
    }
}