using MySql.Data.MySqlClient;

namespace T_API.Core.Exception
{
    public  class MySqlExceptionHandler:ExceptionHandler
    {
        
        
        public static System.Exception HandleMySqlException(System.Exception e)
        {
            var ex = (MySqlException)e;
            
            if (ex.Number == (int)MySqlErrorCode.AbortingConnection ||
                ex.Number == (int)MySqlErrorCode.AccessDenied ||
                ex.Number == (int)MySqlErrorCode.UnableToConnectToHost ||
                ex.Number == (int)MySqlErrorCode.TooManyUserConnections ||
                ex.Number == (int)MySqlErrorCode.NewAbortingConnection ||
                ex.Number == (int)MySqlErrorCode.ConnectionCountError ||
                ex.Number == (int)MySqlErrorCode.AccessDenied)
            {
                return new DatabaseException("Veritabanına bağlantı sırasında hata oluştu", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.CannotAddForeignConstraint)
            {
                return new DatabaseException("Foreign Constraint Eklenirken hata oluştu", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.CannotCreateDatabase)
            {
                return new DatabaseException("Database Oluşturulurken hata oluştu", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.CannotCreateTable)
            {
                return new DatabaseException("Tablo oluşturulurken hata oluştu", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.DuplicateUnique)
            {
                return new DatabaseException("Eklenilen veri için zaten kayıt mevcut", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.RequiresPrimaryKey)
            {
                return new DatabaseException("Primary Key Alanı Boş Geçilemez mevcut", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.MultiplePrimaryKey)
            {
                return new DatabaseException("Primary Key Alanı Tekrar Eden Veri İçeriyor.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.PrimaryCannotHaveNull)
            {
                return new DatabaseException("Primary Key Alanı Null Geçilemez mevcut", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongValueForType)
            {
                return new DatabaseException("Belirtilen tip için yanlış değer atandı.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongColumnName)
            {
                return new DatabaseException("Yanlış Sütün adı girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongArguments)
            {
                return new DatabaseException("Girilen parametreler hatalı.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongDatabaseName)
            {
                return new DatabaseException("Hatalı Veritabanı Adı girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongForeignKeyDefinition)
            {
                return new DatabaseException("Hatalı Foreign Key Adı Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongSizeNumber)
            {
                return new DatabaseException("Hatalı Genişlik Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongTableName)
            {
                return new DatabaseException("Hatalı Tablo Adı Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongStringLength)
            {
                return new DatabaseException("Hatalı String Uzunluğu Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongValue)
            {
                return new DatabaseException("Hatalı Değer Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongUsage)
            {
                return new DatabaseException("Hatalı Kullanım.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongNameForIndex)
            {
                return new DatabaseException("Hatalı Index Ismi Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongNameForCatalog)
            {
                return new DatabaseException("Hatalı Catalog Ismi Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongValueForVariable)
            {
                return new DatabaseException("Belirtilen değişken için hatalı veri Girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.WrongTypeForVariable)
            {
                return new DatabaseException("Belirtilen değişken için hatalı veri tipi girildi.", ex.InnerException);
            }

            if (ex.Number == (int)MySqlErrorCode.StoredProcedureWrongName)
            {
                return new DatabaseException("Hatalı Stored Procedure Ismi Girildi.", ex.InnerException);
            }

            return new DatabaseException("Veritabanı Hatası.", ex.InnerException);
        }

    }
}