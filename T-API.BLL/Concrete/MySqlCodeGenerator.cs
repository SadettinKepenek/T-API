using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using T_API.BLL.Abstract;
using T_API.Entity.Concrete;
using Index = T_API.Entity.Concrete.Index;

namespace T_API.BLL.Concrete
{
    public class MySqlCodeGenerator : ISqlCodeGenerator
    {
        public string GenerateCreateDatabaseQuery(Database database)
        {
            string s = $"CREATE DATABASE {database.DatabaseName};" +
            $"USE {database.DatabaseName};" +
            $"CREATE USER '{database.Username}'@'localhost' IDENTIFIED BY '{database.Password}';" +
            $"GRANT ALL PRIVILEGES ON {database.DatabaseName}.* to '{database.Username}'@'localhost';";
            return s;
        }
        public string GenerateCreateTableQuery(Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"USE {table.DatabaseName}; \n");
            sb.AppendLine($"Create Table {table.TableName}");
            sb.AppendLine("(");

            #region Columns

            foreach (Column column in table.Columns)
            {

                if (table.Indices == null) table.Indices = new ObservableCollection<Index>();


                if (column.Unique)
                    table.Indices.Add(new Index
                    {
                        TableName = table.TableName,
                        IndexColumn = column.ColumnName,
                        IndexName = $"Unique_Index_{table.TableName}_{column.ColumnName}",
                        IsUnique = column.Unique,

                    });


                StringBuilder stringBuilder = new StringBuilder();
                column.DataType = column.DataType.ToUpperInvariant();
                CheckColumnIsValid(column);

                stringBuilder.Append($"\t  {column.ColumnName}\t");
                if (!String.IsNullOrEmpty(column.DataType)) stringBuilder.Append($"{column.DataType}");
                if (column.HasLength) stringBuilder.Append($"({column.DataLength})");
                else stringBuilder.Append(" ");

                if (column.DefaultValue != null)
                    stringBuilder.Append($"\tdefault {column.DefaultValue}");

                if (column.AutoInc)
                {
                    if (column.DefaultValue == null)
                    {
                        stringBuilder.Append(" auto_increment");
                    }
                    else
                    {
                        throw new AmbiguousMatchException("Auto Increment mevcut iken Default value verilemez");
                    }
                }
                if (column.PrimaryKey)
                {
                    stringBuilder.Append("\tprimary key");
                }
                else
                {
                    stringBuilder.Append(column.NotNull ? "\tnot null" : "\t null");
                }

                string columnQuery = stringBuilder.ToString();
                sb.Append(columnQuery);
                if (table.Columns.IndexOf(column) != table.Columns.Count - 1)
                    sb.Append(",\n");
            }

            #endregion

            #region ForeignKeys

            if (table.ForeignKeys != null && table.ForeignKeys.Count != 0)
            {
                sb.AppendLine(",");
                foreach (ForeignKey foreignKey in table.ForeignKeys)
                {
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append($"\tconstraint {foreignKey.ForeignKeyName} foreign key ({foreignKey.SourceColumn}) " +
                               $"references {foreignKey.TargetTable} ({foreignKey.TargetColumn})");
                    string foreignKeyQuery = sb1.ToString();
                    sb.Append(foreignKeyQuery);
                    if (table.ForeignKeys.IndexOf(foreignKey) != table.ForeignKeys.Count - 1)
                        sb.Append(",\n");
                }
            }

            #endregion

            #region Indices

            if (table.Indices != null && table.Indices.Where(x => x.IsUnique).ToList().Count != 0)
            {
                sb.AppendLine(",");
                foreach (Index index in table.Indices.Where(x => x.IsUnique))
                {
                    StringBuilder sb1 = new StringBuilder();

                    sb1.Append(index.IsUnique
                        ? $"\tconstraint {index.IndexName} unique ({index.IndexColumn})"
                        : $"\tcreate index {index.IndexName} on {index.TableName} ({index.IndexColumn});");
                    string indexQuery = sb1.ToString();
                    sb.Append(indexQuery);
                    if (table.Indices.IndexOf(index) != table.Indices.Where(x => x.IsUnique).ToList().Count - 1)
                        sb.Append(",\n");
                }
            }

            #endregion

            #region Keys

            if (table.Keys != null && table.Keys.Count != 0)
            {
                if (table.Keys.Any(x => x.IsPrimary) && table.Columns.Any(x => x.PrimaryKey))
                {
                    throw new AmbiguousMatchException($"Bir tablo sadece bir adet primary key içerebilir");
                }

                sb.AppendLine(",");
                foreach (Key key in table.Keys)
                {
                    StringBuilder sb1 = new StringBuilder();
                    if (!key.IsPrimary)
                        sb1.Append($"\tconstraint {key.KeyName} unique ({key.KeyColumn})");
                    string keyQuery = sb1.ToString();
                    sb.Append(keyQuery);
                    if (table.Keys.IndexOf(key) != table.Keys.Count - 1)
                        sb.Append(",\n");
                }
            }

            #endregion

            sb.AppendLine("\n);");

            #region NonUniqueIndices

            if (table.Indices != null && table.Indices.Where(x => x.IsUnique == false).ToList().Count != 0)
            {
                foreach (Index index in table.Indices.Where(x => x.IsUnique == false))
                {
                    StringBuilder sb1 = new StringBuilder();

                    sb1.Append(index.IsUnique
                        ? $"\tconstraint {index.IndexName} unique ({index.IndexColumn})"
                        : $"\tcreate index {index.IndexName} on {index.TableName} ({index.IndexColumn});");
                    string indexQuery = sb1.ToString();
                    sb.Append(indexQuery);
                    if (table.Indices.IndexOf(index) != table.Indices.Count - 1)
                        sb.Append("\n");
                }
            }

            #endregion

            return sb.ToString(); ;
        }
        public string GenerateDropTableQuery(Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"drop table if exists {table.TableName} cascade;");
            return sb.ToString();
        }
        public string GenerateAddColumnQuery(Column column, Table table)
        {
            #region AddColumnStringGenerator

            StringBuilder stringBuilder = new StringBuilder();
            column.DataType = column.DataType.ToUpperInvariant();
            CheckColumnIsValid(column);
            stringBuilder.Append($"\t add {column.ColumnName}\t");
            if (!String.IsNullOrEmpty(column.DataType)) stringBuilder.Append($"{column.DataType}");
            stringBuilder.Append(column.HasLength ? $"({column.DataLength})" : " ");
            if (column.DefaultValue != null)
                stringBuilder.Append($"\tdefault {column.DefaultValue}");

            if (column.AutoInc)
            {
                if (column.DefaultValue == null)
                {
                    stringBuilder.Append(" auto_increment");
                }
                else
                {
                    throw new AmbiguousMatchException("Auto Increment mevcut iken Default value verilemez");
                }
            }

            if (column.PrimaryKey)
            {
                stringBuilder.Append("\tprimary key");
            }
            else
            {
                stringBuilder.Append(column.NotNull ? "\tnot null" : "\t null");
            }

            #endregion
            var addColumnString = stringBuilder.ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"USE {table.DatabaseName}; \n");
            sb.AppendLine($"Alter Table {table.TableName}");
            sb.AppendLine(addColumnString);
            sb.Append(";\n");

            if (column.Unique)
            {
                var index = new Index
                {
                    TableName = table.TableName,
                    IndexColumn = column.ColumnName,
                    IndexName = $"Unique_Index_{table.TableName}_{column.ColumnName}",
                    IsUnique = column.Unique,

                };
                sb.AppendLine(GenerateAddIndexQuery(index, table));
            }


            return sb.ToString();
        }
        public string GenerateAddIndexQuery(Index index, Table table)
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(index.IsUnique
                ? $"\t create unique index {index.IndexName} on {index.TableName} ({index.IndexColumn})"
                : $"\t create index {index.IndexName} on {index.TableName} ({index.IndexColumn})");
            var addIndexStr = stringBuilder.ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"USE {table.DatabaseName}; \n");
            sb.AppendLine(addIndexStr);
            return sb.ToString();
        }
        public string GenerateAddForeignKeyQuery(ForeignKey foreignKey, Table table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"USE {table.DatabaseName}; \n");
            stringBuilder.AppendLine($"Alter Table {table.TableName}");


            StringBuilder sb = new StringBuilder();
            sb.Append($"\t add constraint {foreignKey.ForeignKeyName} foreign key ({foreignKey.SourceColumn}) " +
                      $"references {foreignKey.TargetTable} ({foreignKey.TargetColumn})");
            var foreignStr = sb.ToString();

            stringBuilder.AppendLine(foreignStr);
            stringBuilder.Append(";\n");

            return stringBuilder.ToString();
        }
        public string GenerateAddKeyQuery(Key key, Table table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"USE {table.DatabaseName}; \n");
            stringBuilder.AppendLine($"Alter Table {table.TableName}");

            StringBuilder sb = new StringBuilder();
            if (!key.IsPrimary)
                sb.Append($"\t add constraint {key.KeyName} unique ({key.KeyColumn})");
            var keyStr = sb.ToString();

            stringBuilder.AppendLine(keyStr);
            stringBuilder.Append(";\n");

            return stringBuilder.ToString();
        }
        public string GenerateModifyColumnQuery(Column column, Table table)
        {
            #region ModifyColumnStrGenerator

            StringBuilder stringBuilder = new StringBuilder();
            column.DataType = column.DataType.ToUpperInvariant();
            CheckColumnIsValid(column);


            stringBuilder.Append($"\t modify {column.ColumnName}\t");
            if (!String.IsNullOrEmpty(column.DataType)) stringBuilder.Append($"{column.DataType}");
            if (column.HasLength) stringBuilder.Append($"({column.DataLength})");
            else stringBuilder.Append(" ");

            if (column.DefaultValue != null)
                stringBuilder.Append($"\tdefault {column.DefaultValue}");


            if (column.AutoInc)
            {
                if (column.DefaultValue == null)
                {
                    stringBuilder.Append(" auto_increment");
                }
                else
                {
                    throw new AmbiguousMatchException("Auto Increment mevcut iken Default value verilemez");
                }
            }

            if (column.PrimaryKey)
            {
                stringBuilder.Append("\tprimary key");
            }
            else
            {
                if (column.NotNull)
                {
                    stringBuilder.Append("\tnot null");
                }
                else
                {
                    stringBuilder.Append("\t null");
                }
            }

            var modifyColumnStr = stringBuilder.ToString();

            #endregion

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"USE {table.DatabaseName}; \n");
            sb.AppendLine($"Alter Table {table.TableName}");
            sb.AppendLine(modifyColumnStr);
            sb.Append(";\n");

            return sb.ToString();
        }
        public string GenerateModifyIndexQuery(Index index, Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"USE {table.DatabaseName}; \n");
            sb.AppendLine($"\n");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(index.IsUnique
                ? $"\t create unique index {index.IndexName} on {index.TableName} ({index.IndexColumn})"
                : $"\t create index {index.IndexName} on {index.TableName} ({index.IndexColumn})");
            var modifyIndexStr = stringBuilder.ToString();
            sb.AppendLine(modifyIndexStr);
            sb.AppendLine($"\n");

            return sb.ToString();
        }
        public string GenerateModifyForeignKeyQuery(ForeignKey foreignKey, Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"USE {table.DatabaseName}; \n");
            sb.AppendLine($"Alter Table {table.TableName}");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"\t add constraint {foreignKey.ForeignKeyName} foreign key ({foreignKey.SourceColumn}) " +
                                 $"references {foreignKey.TargetTable} ({foreignKey.TargetColumn})");
            var modifyForeignKeyStr = stringBuilder.ToString();
            sb.AppendLine(modifyForeignKeyStr);
            sb.Append(";\n");
            return sb.ToString();
        }
        public string GenerateModifyKeyQuery(Key key, Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"USE {table.DatabaseName}; \n");
            sb.AppendLine($"Alter Table {table.TableName}");
            StringBuilder stringBuilder = new StringBuilder();
            if (!key.IsPrimary)
                stringBuilder.Append($"\t add constraint {key.KeyName} unique ({key.KeyColumn})");
            var modifyKeyStr = stringBuilder.ToString();
            sb.AppendLine(modifyKeyStr);
            sb.Append(";\n");
            return sb.ToString();
        }
        private static void CheckColumnIsValid(Column column)
        {
            if ((column.DataType.Equals("TEXT") || column.DataType.Equals("TINYTEXT") || column.DataType.Equals("CHAR") ||
                 column.DataType.Equals("VARCHAR") || column.DataType.Equals("LONGTEXT") ||
                 column.DataType.Equals("MEDIUMTEXT")))
            {
                if ((!column.HasLength || column.DataLength == 0))
                {
                    throw new ArgumentException("Metin tipindeki columnlar için geçerli bir size girilmedi");
                }

                if (column.DefaultValue != null && column.DefaultValue.GetType() != typeof(String))
                {
                    throw new ArgumentNullException("DefaultValue", "Default value için geçerli bir tip verilmedi");
                }

                if (column.PrimaryKey)
                    throw new ArgumentNullException("DefaultValue", "String Veri tipi Auto Inc olarak belirlenemez");
            }
            else if ((column.DataType.Equals("DATE") || column.DataType.Equals("TIME") || column.DataType.Equals("DATETIME") ||
                      column.DataType.Equals("YEAR") || column.DataType.Equals("TIMESTAMP")))
            {
                if ((column.HasLength || column.DataLength != 0))
                {
                    throw new ArgumentException("Tarih tipindeki columnlar için size girilemez.");
                }

                if (column.DefaultValue != null && (column.DefaultValue.GetType() != typeof(DateTime) ||
                                                    column.DefaultValue.GetType() != typeof(TimeSpan)))
                {
                    throw new ArgumentNullException("DefaultValue", "Default value için geçerli bir tip verilmedi");
                }

                if (column.PrimaryKey || column.AutoInc || column.Unique)
                    throw new ArgumentNullException("DefaultValue", "String Veri tipi Auto Inc olarak belirlenemez");
            }
            else if (column.DataType.Equals("INT") || column.DataType.Equals("SMALLINT") || column.DataType.Equals("BIGINT") ||
                     column.DataType.Equals("FLOAT") || column.DataType.Equals("DOUBLE") || column.DataType.Equals("DECIMAL"))
            {
                if ((column.HasLength || column.DataLength != 0))
                {
                    throw new ArgumentException("Column türü için size belirtilemez");
                }

                if (column.DefaultValue != null &&
                    (column.DefaultValue.GetType() != typeof(int) || column.DefaultValue.GetType() != typeof(double)
                                                                  || column.DefaultValue.GetType() != typeof(decimal) ||
                                                                  column.DefaultValue.GetType() != typeof(float)))
                {
                    throw new ArgumentNullException("DefaultValue", "Default value için geçerli bir tip verilmedi");
                }
            }
            else if (column.DataType.Equals("BOOL") || column.DataType.Equals("BOOLEAN"))
            {
                if ((column.HasLength || column.DataLength != 0))
                {
                    throw new ArgumentException("Column türü için size belirtilemez");
                }

                if (column.DefaultValue != null && column.DefaultValue.GetType() != typeof(bool))
                {
                    throw new ArgumentNullException("DefaultValue", "Default value için geçerli bir tip verilmedi");
                }

                if (column.AutoInc || column.PrimaryKey || column.Unique)
                    throw new ArgumentNullException("DefaultValue",
                        "BOOL Veri tipi Auto Inc,Unique veya Primary Key olarak belirlenemez");
            }
            else
            {
                throw new ArgumentOutOfRangeException("DataType", "Column için belirtilen column tipi hatalı.");
            }
        }
        public string GenerateDropColumnQuery(Column column)
        {
            if (column.PrimaryKey)
            {
                throw new ArgumentException("Primary Key Column Drop edilemez");
            }


            throw new System.NotImplementedException();
        }
        public string GenerateDropRelationQuery(ForeignKey foreignKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\t alter table {foreignKey.SourceTable} drop foreign key {foreignKey.ForeignKeyName};");
            return sb.ToString();
        }
        public string GenerateDropKeyQuery(Key key)
        {
            StringBuilder sb = new StringBuilder();
            if (!key.IsPrimary)
                sb.Append($"\t alter table {key.TableName} drop key {key.KeyName};");
            else
            {
                sb.Append($"\t alter table {key.TableName} drop primary key");
            }
            return sb.ToString();
        }
        public string GenerateDropIndexQuery(Index index)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"\t drop index {index.IndexName} on {index.TableName};");

            return sb.ToString();
        }

        public void Dispose()
        {
        }
    }
}