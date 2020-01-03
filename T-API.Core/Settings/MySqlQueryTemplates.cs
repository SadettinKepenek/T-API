using System;

namespace T_API.Core.Settings
{
    public class MySqlQueryTemplates
    {
        public static string GetTable(string databaseName)
        {
            string query = " SELECT " +
                           " TABLES.TABLE_NAME," +
                           " COLUMNS.COLUMN_NAME," +
                           " COLUMNS.COLUMN_TYPE," +
                           " COLUMNS.COLUMN_DEFAULT," +
                           " COLUMNS.IS_NULLABLE," +
                           " COLUMNS.CHARACTER_MAXIMUM_LENGTH," +
                           " COLUMNS.NUMERIC_PRECISION," +
                           " COLUMNS.EXTRA,"+
                           " IF(COLUMNS.NUMERIC_PRECISION IS NULL, false, true) AS IsNumeric," +
                           " COLUMNS.COLUMN_KEY," +
                           " KEY_COLUMN_USAGE.CONSTRAINT_NAME," +
                           " KEY_COLUMN_USAGE.REFERENCED_TABLE_NAME," +
                           " KEY_COLUMN_USAGE.REFERENCED_COLUMN_NAME," +
                           " IF(KEY_COLUMN_USAGE.REFERENCED_COLUMN_NAME IS NULL, false, true) AS IsForeignKey," +
                           " STATISTICS.INDEX_NAME," +
                           " STATISTICS.INDEX_TYPE," +
                           " STATISTICS.NON_UNIQUE" +
                           " from information_schema.tables" +
                           " inner join information_schema.COLUMNS on TABLES.TABLE_NAME = COLUMNS.TABLE_NAME" +
                           " LEFT join information_schema.KEY_COLUMN_USAGE on COLUMNS.COLUMN_NAME = KEY_COLUMN_USAGE.COLUMN_NAME" +
                           " LEFT JOIN information_schema.STATISTICS on COLUMNS.COLUMN_NAME = STATISTICS.COLUMN_NAME" +
                           $" where TABLES.TABLE_SCHEMA = '{databaseName}' AND information_schema.COLUMNS.TABLE_SCHEMA='{databaseName}'";
            return query;
        }
        public static string GetTable(string databaseName, string tableName)
        {
            string query = " SELECT " +
                           " TABLES.TABLE_NAME," +
                           " COLUMNS.COLUMN_NAME," +
                           " COLUMNS.COLUMN_TYPE," +
                           " COLUMNS.COLUMN_DEFAULT," +
                           " COLUMNS.IS_NULLABLE," +
                           " COLUMNS.CHARACTER_MAXIMUM_LENGTH," +
                           " COLUMNS.NUMERIC_PRECISION," +
                           " COLUMNS.EXTRA," +
                           " IF(COLUMNS.NUMERIC_PRECISION IS NULL, false, true) AS IsNumeric," +
                           " COLUMNS.COLUMN_KEY," +
                           " KEY_COLUMN_USAGE.CONSTRAINT_NAME," +
                           " KEY_COLUMN_USAGE.REFERENCED_TABLE_NAME," +
                           " KEY_COLUMN_USAGE.REFERENCED_COLUMN_NAME," +
                           " IF(KEY_COLUMN_USAGE.REFERENCED_COLUMN_NAME IS NULL, false, true) AS IsForeignKey," +
                           " STATISTICS.INDEX_NAME," +
                           " STATISTICS.INDEX_TYPE," +
                           " STATISTICS.NON_UNIQUE" +
                           " from information_schema.tables" +
                           " inner join information_schema.COLUMNS on TABLES.TABLE_NAME = COLUMNS.TABLE_NAME" +
                           " LEFT join information_schema.KEY_COLUMN_USAGE on COLUMNS.COLUMN_NAME = KEY_COLUMN_USAGE.COLUMN_NAME" +
                           " LEFT JOIN information_schema.STATISTICS on COLUMNS.COLUMN_NAME = STATISTICS.COLUMN_NAME" +
                           $" where TABLES.TABLE_SCHEMA = '{databaseName}' and TABLES.TABLE_NAME = '{tableName}'";
            return query;
        }
        public static string GetColumn(string databaseName, string tableName, string columnName)
        {
            string query = "SELECT " +
                           "TABLES.TABLE_NAME," +
                           "COLUMNS.COLUMN_NAME," +
                           "COLUMNS.COLUMN_TYPE," +
                           "COLUMNS.COLUMN_DEFAULT," +
                           "COLUMNS.IS_NULLABLE," +
                           "COLUMNS.CHARACTER_MAXIMUM_LENGTH," +
                           "COLUMNS.NUMERIC_PRECISION," +
                           "COLUMNS.EXTRA," +
                           "IF(COLUMNS.NUMERIC_PRECISION IS NULL, false, true) AS IsNumeric," +
                           "COLUMNS.COLUMN_KEY," +
                           "KEY_COLUMN_USAGE.CONSTRAINT_NAME," +
                           "KEY_COLUMN_USAGE.REFERENCED_TABLE_NAME," +
                           "KEY_COLUMN_USAGE.REFERENCED_COLUMN_NAME," +
                           "IF(KEY_COLUMN_USAGE.REFERENCED_COLUMN_NAME IS NULL, false, true) AS IsForeignKey," +
                           "STATISTICS.INDEX_NAME," +
                           "STATISTICS.INDEX_TYPE," +
                           "STATISTICS.NON_UNIQUE" +
                           "from information_schema.tables" +
                           "inner join information_schema.COLUMNS on TABLES.TABLE_NAME = COLUMNS.TABLE_NAME" +
                           "LEFT join information_schema.KEY_COLUMN_USAGE on COLUMNS.COLUMN_NAME = KEY_COLUMN_USAGE.COLUMN_NAME" +
                           " LEFT JOIN information_schema.STATISTICS on COLUMNS.COLUMN_NAME = STATISTICS.COLUMN_NAME" +
                           $"where TABLES.TABLE_SCHEMA = '{databaseName}' and TABLES.TABLE_NAME = '{tableName}'" +
                           $"and Columns.COLUMN_NAME = '{columnName}'";
            return query;
        }
    }
}