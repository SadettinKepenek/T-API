using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using T_API.Core.DAL.Concrete;
using T_API.Core.Exception;
using T_API.Core.Settings;
using T_API.DAL.Abstract;
using T_API.Entity.Concrete;
using Index = T_API.Entity.Concrete.Index;

namespace T_API.DAL.Concrete
{
    public class MySqlRealDbRepository : IRealDbRepository
    {





        /// <summary>
        /// Veritabanında istenilen sorguyu çalıştırır.
        /// </summary>
        /// <param name="query">Çalıştırılmak istenilen sorgu</param>
        /// <param name="dbInformation">Bağlantı bilgileri</param>
        /// <returns></returns>
        public async Task ExecuteQueryOnRemote(string query, DbInformation dbInformation)
        {
            using (var conn = DbConnectionFactory.CreateConnection(dbInformation))
            {

                var cmd = conn.CreateCommand(query);

                using (cmd)
                {

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                            Console.WriteLine("  Message: {0}", ex.Message);
                            //transaction.Rollback();
                            throw ExceptionHandler.HandleException(ex);

                        }

                }

            }
        }

        /// <summary>
        /// Ana makinede istenilen sorguyu çalıştırır.
        /// </summary>
        /// <param name="query">Çalıştırılmak istenilen sorgu.</param>
        /// <returns></returns>
        public async Task ExecuteQueryOnRemote(string query)
        {
            using (var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
            {


                var cmd = conn.CreateCommand(query);

                using (cmd)
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                        throw ExceptionHandler.HandleException(ex);

                    }
                }

            }

        }

        /// <summary>
        /// Veritabanının tablolarını , foreign keylerini,indexlerini,keylerini ve sütunlarını getirir.
        /// </summary>
        /// <param name="databaseName">Veritabanı adı</param>
        /// <returns>Veritabanı Tabloları</returns>
        public async Task<List<Table>> GetTables(string databaseName)
        {
            using (var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
            {


                var cmd = conn.CreateCommand(MySqlQueryTemplates.GetTable(databaseName));

                using (cmd)
                {
                    try
                    {
                        var sqlReader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(sqlReader);
                        List<Table> tables = new List<Table>();


                        if (dt.Rows.Count == 0)
                        {
                            return tables;
                        }

                        var groupedTables = dt.AsEnumerable().GroupBy(row => row.Field<string>("TABLE_NAME"));
                        foreach (var group in groupedTables)
                        {

                            if (tables.All(x => x.TableName != group.Key))
                            {
                                var table = ParseTable(group);
                                table.DatabaseName = databaseName;
                                tables.Add(table);
                            }
                        }
                        return tables;
                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }
                }

            }
        }

        private static Table ParseTable(IGrouping<string, DataRow> group)
        {
            Table table = new Table
            {
                TableName = group.Key
            };
            var groupedColumns = group.AsEnumerable().GroupBy(row => row.Field<string>("COLUMN_NAME"));
            //Sütun isimlerine göre gruplandırıldı şimdi sütunlar gezilecek.
            foreach (IGrouping<string, DataRow> groupedColumn in groupedColumns)
            {
                #region Columns

                var column = ParseColumn(groupedColumn);
                table.Columns.Add(column);

                #endregion

                #region ForeignKeys

                var foreignKeys = groupedColumn.Where(x => x.Field<long>("IsForeignKey") == 1);
                foreach (DataRow key in foreignKeys)
                {
                    if (table.ForeignKeys.All(x => x.ForeignKeyName != key["CONSTRAINT_NAME"] as string))
                    {
                        var foreignKey = ParseForeignKey(key);
                        table.ForeignKeys.Add(foreignKey);
                    }
                }

                #endregion

                #region UniqueKeys And Primary Keys

                var uniqueKeys =
                    groupedColumn.Where(x =>
                        x.Field<string>("COLUMN_KEY").Equals("UNI") || x.Field<string>("COLUMN_KEY").Equals("PRI"));
                foreach (DataRow uniqueKey in uniqueKeys)
                {
                    if (table.Keys.All(x => x.KeyName != uniqueKey["CONSTRAINT_NAME"] as string))
                    {
                        var key = ParseUniqueKey(uniqueKey);
                        table.Keys.Add(key);
                    }
                }

                #endregion
            }

            return table;
        }

        private static Table ParseTable(DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return null;

            Table table = new Table
            {
                TableName = dt.Rows[0]["TABLE_NAME"] as string
            };
            var groupedColumns = dt.AsEnumerable().GroupBy(row => row.Field<string>("COLUMN_NAME"));
            //Sütun isimlerine göre gruplandırıldı şimdi sütunlar gezilecek.
            foreach (IGrouping<string, DataRow> groupedColumn in groupedColumns)
            {
                #region Columns

                var column = ParseColumn(groupedColumn);
                table.Columns.Add(column);

                #endregion

                #region ForeignKeys

                var foreignKeys = groupedColumn.Where(x => x.Field<Int64>("IsForeignKey") == 1);
                foreach (DataRow key in foreignKeys)
                {
                    if (table.ForeignKeys.All(x => x.ForeignKeyName != key["CONSTRAINT_NAME"] as string))
                    {
                        var foreignKey = ParseForeignKey(key);
                        table.ForeignKeys.Add(foreignKey);
                    }
                }

                #endregion

                #region UniqueKeys And Primary Keys

                var uniqueKeys =
                    groupedColumn.Where(x =>
                        x.Field<string>("COLUMN_KEY").Equals("UNI") || x.Field<string>("COLUMN_KEY").Equals("PRI"));
                foreach (DataRow uniqueKey in uniqueKeys)
                {
                    if (table.Keys.All(x => x.KeyName != uniqueKey["CONSTRAINT_NAME"] as string))
                    {
                        var key = ParseUniqueKey(uniqueKey);
                        table.Keys.Add(key);
                    }
                }

                #endregion
            }

            return table;
        }
        private static Column ParseColumn(IGrouping<string, DataRow> groupedColumn)
        {
            var firstRow = groupedColumn.FirstOrDefault();
            Column column = new Column
            {
                DefaultValue = firstRow["COLUMN_DEFAULT"],
                ColumnName = firstRow["COLUMN_NAME"] as string,
                DataLength = Convert.ToInt32(firstRow["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value
                    ? firstRow["CHARACTER_MAXIMUM_LENGTH"]
                    : 0),
                DataType = firstRow["COLUMN_TYPE"] as string,
                HasLength = firstRow["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value,
                NotNull = firstRow["IS_NULLABLE"].Equals("NO"),
                TableName = firstRow["TABLE_NAME"] as string,
                PrimaryKey = groupedColumn.Any(x => x.Field<string>("COLUMN_KEY").Equals("PRI")),
                Unique = groupedColumn.Any(x => x.Field<string>("COLUMN_KEY").Equals("UNI")),
                AutoInc = groupedColumn.Any(x => x.Field<string>("EXTRA").Contains("auto_increment"))
            };
            return column;
        }
        private static ForeignKey ParseForeignKey(DataRow key)
        {
            ForeignKey foreignKey = new ForeignKey();
            foreignKey.ForeignKeyName = key["CONSTRAINT_NAME"] as string;
            foreignKey.TargetColumn = key["REFERENCED_COLUMN_NAME"] as string;
            foreignKey.SourceTable = key["TABLE_NAME"] as string;
            foreignKey.TargetTable = key["REFERENCED_TABLE_NAME"] as string;
            foreignKey.SourceColumn = key["COLUMN_NAME"] as string;
            return foreignKey;
        }
        private static Key ParseUniqueKey(DataRow uniqueKey)
        {
            Key key = new Key();
            var uniqueKeyName = uniqueKey["CONSTRAINT_NAME"] as string;
            key.TableName = uniqueKey["TABLE_NAME"] as string;
            key.IsPrimary = uniqueKey["COLUMN_KEY"].Equals("PRI");
            key.KeyColumn = uniqueKey["COLUMN_NAME"] as string;
            key.KeyName = uniqueKeyName;
            return key;
        }
        public async Task<Table> GetTable(string tableName, string databaseName)
        {
            using (var conn = DbConnectionFactory.CreateConnection(ConfigurationSettings.ServerDbInformation))
            {

                var cmd = conn.CreateCommand(MySqlQueryTemplates.GetTable(databaseName, tableName));

                using (cmd)
                {
                    try
                    {
                        var sqlReader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(sqlReader);

                        var table = ParseTable(dt);
                        table.DatabaseName = databaseName;
                        return table;
                    }
                    catch (Exception ex)
                    {
                        throw ExceptionHandler.HandleException(ex);

                    }
                }

            }
        }
        public Task<List<ForeignKey>> GetForeignKeys(string databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<List<ForeignKey>> GetForeignKeys(string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<ForeignKey> GetForeignKey(string databaseName, string tableName, string foreignKeyName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Key>> GetKeys(string databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Key>> GetKeys(string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<Key> GetKey(string databaseName, string tableName, string keyName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Index>> GetIndices(string databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Index>> GetIndices(string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<Index> GetIndex(string databaseName, string tableName, string indexName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Column>> GetColumns(string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<Column> GetColumn(string databaseName, string tableName, string columnName)
        {
            throw new NotImplementedException();
        }
    }
}