using System;
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
        public string CreateTable(Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Create Table '{table.TableName}'");
            sb.AppendLine("(");
            foreach (Column column in table.Columns)
            {
                string columnQuery = CreateColumn(column);
                sb.Append(columnQuery);
                if (table.Columns.IndexOf(column) != table.Columns.Count - 1)
                    sb.Append(",\n");
            }

            if (table.ForeignKeys != null && table.ForeignKeys.Count != 0)
            {
                sb.AppendLine(",");
                foreach (ForeignKey foreignKey in table.ForeignKeys)
                {
                    string foreignKeyQuery = CreateRelation(foreignKey);
                    sb.Append(foreignKeyQuery);
                    if (table.ForeignKeys.IndexOf(foreignKey) != table.ForeignKeys.Count - 1)
                        sb.Append(",\n");
                }
            }
            if (table.Indices != null && table.Indices.Where(x => x.IsUnique).ToList().Count != 0)
            {
                sb.AppendLine(",");
                foreach (Index index in table.Indices.Where(x => x.IsUnique))
                {
                    string indexQuery = CreateIndex(index);
                    sb.Append(indexQuery);
                    if (table.Indices.IndexOf(index) != table.Indices.Where(x => x.IsUnique).ToList().Count - 1)
                        sb.Append(",\n");
                }
            }

            if (table.Keys != null && table.Keys.Count != 0)
            {
                if (table.Keys.Any(x => x.IsPrimary) && table.Columns.Any(x => x.PrimaryKey))
                {
                    throw new AmbiguousMatchException($"Bir tablo sadece bir adet primary key içerebilir");
                }

                sb.AppendLine(",");
                foreach (Key key in table.Keys)
                {
                    string keyQuery = CreateKey(key);
                    sb.Append(keyQuery);
                    if (table.Keys.IndexOf(key) != table.Keys.Count - 1)
                        sb.Append(",\n");
                }
            }

            sb.AppendLine("\n);");
            if (table.Indices != null && table.Indices.Where(x => x.IsUnique == false).ToList().Count != 0)
            {
                foreach (Index index in table.Indices.Where(x => x.IsUnique == false))
                {
                    string indexQuery = CreateIndex(index);
                    sb.Append(indexQuery);
                    if (table.Indices.IndexOf(index) != table.Indices.Count - 1)
                        sb.Append("\n");
                }
            }


            Console.WriteLine(sb.ToString());
            return String.Empty;
        }

        public string DropTable(Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"drop table if exists {table.TableName} cascade;");
            return sb.ToString();
        }

        public string AlterTable(Table table)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Alter Table {table.TableName}");
            foreach (Column column in table.Columns)
            {
                string columnQuery = AlterColumn(column);
                sb.Append($"{columnQuery}");
                if (table.Columns.IndexOf(column) != table.Columns.Count - 1)
                    sb.Append(",\n");
            }

            sb.Append(";\n");

            sb.AppendLine($"Alter Table {table.TableName}");
            if (table.ForeignKeys != null && table.ForeignKeys.Count != 0)
            {
                foreach (ForeignKey foreignKey in table.ForeignKeys)
                {
                    string foreignKeyQuery = AlterRelation(foreignKey);
                    sb.Append(foreignKeyQuery);
                    if (table.ForeignKeys.IndexOf(foreignKey) != table.ForeignKeys.Count - 1)
                        sb.Append(",\n");
                }
            }
            sb.Append(";\n");

            sb.AppendLine($"Alter Table {table.TableName}");
            if (table.Indices != null && table.Indices.Where(x => x.IsUnique).ToList().Count != 0)
            {
                foreach (Index index in table.Indices.Where(x => x.IsUnique))
                {
                    string indexQuery = AlterIndex(index);
                    sb.Append(indexQuery);
                    if (table.Indices.IndexOf(index) != table.Indices.Where(x => x.IsUnique).ToList().Count - 1)
                        sb.Append(",\n");
                }
            }
            sb.Append(";\n");

            sb.AppendLine($"Alter Table {table.TableName}");
            if (table.Keys != null && table.Keys.Count != 0)
            {
                if (table.Keys.Any(x => x.IsPrimary) && table.Columns.Any(x => x.PrimaryKey))
                {
                    throw new AmbiguousMatchException($"Bir tablo sadece bir adet primary key içerebilir");
                }

                foreach (Key key in table.Keys)
                {
                    string keyQuery = AlterKey(key);
                    sb.Append(keyQuery);
                    if (table.Keys.IndexOf(key) != table.Keys.Count - 1)
                        sb.Append(",\n");
                }
            }
            sb.Append(";\n");

            sb.AppendLine($"Alter Table {table.TableName}");
            if (table.Indices != null && table.Indices.Where(x => x.IsUnique == false).ToList().Count != 0)
            {
                foreach (Index index in table.Indices.Where(x => x.IsUnique == false))
                {
                    string indexQuery = AlterIndex(index);
                    sb.Append(indexQuery);
                    if (table.Indices.IndexOf(index) != table.Indices.Count - 1)
                        sb.Append("\n");
                }
            }
            sb.Append(";\n");



            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        public string CreateColumn(Column column)
        {
            StringBuilder stringBuilder = new StringBuilder();
            column.DataType = column.DataType.ToUpperInvariant();
            CheckColumnIsValid(column);

            stringBuilder.Append($"\t{column.ColumnName}\t");
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


            return stringBuilder.ToString();
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

        [Obsolete("To Be Added.")]
        public string DropColumn(Column column)
        {
            if (column.PrimaryKey)
            {
                throw new ArgumentException("Primary Key Column Drop edilemez");
            }


            throw new System.NotImplementedException();
        }

        public string AlterColumn(Column column)
        {
            StringBuilder stringBuilder = new StringBuilder();
            column.DataType = column.DataType.ToUpperInvariant();
            CheckColumnIsValid(column);


            stringBuilder.Append($"\t add {column.ColumnName}\t");
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

            return stringBuilder.ToString();
        }

        public string CreateRelation(ForeignKey foreignKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\tconstraint {foreignKey.ForeignKeyName} foreign key ({foreignKey.SourceColumn}) " +
                      $"references {foreignKey.TargetTable} ({foreignKey.TargetColumn})");
            return sb.ToString();
        }

        public string DropRelation(ForeignKey foreignKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\t alter table {foreignKey.SourceTable} drop foreign key {foreignKey.ForeignKeyName};");
            return sb.ToString();
        }

        public string AlterRelation(ForeignKey foreignKey)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"\t add constraint {foreignKey.ForeignKeyName} foreign key ({foreignKey.SourceColumn}) " +
                      $"references {foreignKey.TargetTable} ({foreignKey.TargetColumn})");
            return sb.ToString();
        }

        public string CreateKey(Key key)
        {
            StringBuilder sb = new StringBuilder();
            if (!key.IsPrimary)
                sb.Append($"\tconstraint {key.KeyName} unique ({key.KeyColumn})");
            return sb.ToString();
        }

        public string DropKey(Key key)
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

        public string AlterKey(Key key)
        {
            StringBuilder sb = new StringBuilder();
            if (!key.IsPrimary)
                sb.Append($"\t add constraint {key.KeyName} unique ({key.KeyColumn})");
            return sb.ToString();
        }

        public string CreateIndex(Index index)
        {
            StringBuilder sb = new StringBuilder();

            if (index.IsUnique)
            {
                sb.Append($"\tconstraint {index.IndexName} unique ({index.IndexColumn})");
            }

            else
            {
                sb.Append($"\tcreate index {index.IndexName} on {index.TableName} ({index.IndexColumn});");
            }

            return sb.ToString();
        }

        public string DropIndex(Index index)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"\t drop index {index.IndexName} on {index.TableName};");

            return sb.ToString();
        }

        public string AlterIndex(Index index)
        {
            StringBuilder sb = new StringBuilder();

            if (index.IsUnique)
            {
                sb.Append($"\t create unique index {index.IndexName} on {index.TableName} ({index.IndexColumn})");
            }

            else
            {
                sb.Append($"\tcreate index {index.IndexName} on {index.TableName} ({index.IndexColumn})");
            }

            return sb.ToString();
        }
    }
}