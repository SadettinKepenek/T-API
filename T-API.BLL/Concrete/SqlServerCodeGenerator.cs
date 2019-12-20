using System;
using System.Reflection;
using System.Text;
using T_API.Entity.Concrete;
using T_API.SqlGenerator.Abstract;
using Index = T_API.Entity.Concrete.Index;

namespace T_API.SqlGenerator.Concrete
{
    public class SqlServerCodeGenerator : SqlCodeGenerator
    {
        public override string CreateTable(Table table)
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
            sb.AppendLine("\n);");

            Console.WriteLine(sb.ToString());
            return String.Empty;
        }

        public override string DropTable(Table table)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterTable(Table table)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateColumn(Column column)
        {
            StringBuilder stringBuilder = new StringBuilder();
            column.DataType = column.DataType.ToUpperInvariant();
            if (column.DataType.Equals("CHAR") || column.DataType.Equals("VARCHAR") || column.DataType.Equals("TEXT") || column.DataType.Equals("NCHAR") || column.DataType.Equals("NVARCHAR") || column.DataType.Equals("NTEXT") || column.DataType.Equals("IMAGE"))
            {
                if (!column.HasLength || column.DataLength == 0)
                {
                    throw new ArgumentException("Metin tipindeki columnlar için size girilmedi");
                }
                if (column.DefaultValue != null && column.DefaultValue.GetType() != typeof(String))
                {
                    throw new ArgumentNullException("DefaultValue", "Default value için geçerli bir tip verilmedi");
                }
                if (column.AutoInc)
                    throw new ArgumentNullException("DefaultValue", "String Veri tipi Auto Inc olarak belirlenemez");
            }
            else if (column.DataType.Equals("INT") || column.DataType.Equals("TINYINT") || column.DataType.Equals("SMALLINT") || column.DataType.Equals("BIGINT") || column.DataType.Equals("DECIMAL") || column.DataType.Equals("MONEY") || column.DataType.Equals("SMALLMONEY") || column.DataType.Equals("DECIMAL") || column.DataType.Equals("FLOAT") || column.DataType.Equals("REAL"))
            {
                if (column.DataType.Equals("DECIMAL") || column.DataType.Equals("NUMERIC"))
                {
                    if (!column.HasLength || column.DataLength == 0)
                    {
                        throw new ArgumentException($"{column.ColumnName} column'u için veri tipine ait size girilmedi");
                    }
                }
                if ((column.HasLength || column.DataLength != 0))
                {
                    throw new ArgumentException($"{column.ColumnName} column'u için size girilemez");

                }
                if (column.DefaultValue != null && (column.DefaultValue.GetType() != typeof(int) || column.DefaultValue.GetType() != typeof(decimal) || column.DefaultValue.GetType() != typeof(float) || column.DefaultValue.GetType() != typeof(double)))
                {
                    throw new ArgumentNullException("DefaultValue", $" {column.ColumnName} için default value'ya ait geçerli bir tip verilmedi");

                }


            }
            else if (column.DataType.Equals("BOOL") || column.DataType.Equals("BOOLEAN") || column.DataType.Equals("BIT"))
            {
                if ((column.HasLength || column.DataLength != 0))
                {
                    throw new ArgumentException("Column türü için size belirtilemez");
                }
                if (column.DefaultValue != null && column.DefaultValue.GetType() != typeof(bool))
                {
                    throw new ArgumentNullException("DefaultValue", $" {column.ColumnName} için default value'ya ait geçerli bir tip verilmedi");

                }
                if (column.AutoInc || column.PrimaryKey || column.Unique)
                    throw new ArgumentNullException("DefaultValue", "BOOL Veri tipi Auto Inc,Unique veya Primary Key olarak belirlenemez");
            }
            else if ((column.DataType.Equals("DATE") || column.DataType.Equals("TIME") || column.DataType.Equals("DATETIME") || column.DataType.Equals("DATETIME2") || column.DataType.Equals("SMALLDATETIME")))
            {
                if ((column.HasLength || column.DataLength != 0))
                {
                    throw new ArgumentException("Tarih tipindeki columnlar için size girilemez.");
                }
                if (column.DefaultValue != null && (column.DefaultValue.GetType() != typeof(DateTime) ||
                                                    column.DefaultValue.GetType() != typeof(TimeSpan)))
                {
                    throw new ArgumentNullException("DefaultValue", $" {column.ColumnName} için default value'ya ait geçerli bir tip verilmedi");

                }
                if (column.PrimaryKey || column.AutoInc || column.Unique)
                    throw new ArgumentNullException("DefaultValue", "String Veri tipi Auto Inc olarak belirlenemez");
            }
            else
            {
                throw new ArgumentOutOfRangeException("DataType", $" {column.ColumnName} için belirtilen column tipi hatalı.");
            }
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
            }


            return stringBuilder.ToString();
        }

        public override string DropColumn(Column column)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterColumn(Column column)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateRelation(ForeignKey foreignKey)
        {
            try
            {
                if (foreignKey.SourceTable != null && foreignKey.SourceTable != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("FOREIGN KEY REFERENCES ");
                    stringBuilder.Append($"{foreignKey.SourceTable}({foreignKey.SourceColumn})");
                    return stringBuilder.ToString();
                }
                throw new Exception("Kaynak tablo veya column bulunamadı");

            }
            catch (Exception e)
            {
                throw new Exception("Relation Oluşturulurken hata");
            }

        }

        public override string DropRelation(ForeignKey foreignKey)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterRelation(ForeignKey foreignKey)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateKey(Key key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (key.KeyName.Equals("FOREIGN KEY"))
            {

            }
            stringBuilder.Append($" {key.KeyColumn}");
            return null;
        }

        public override string DropKey(Key key)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterKey(Key key)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateIndex(Index index)
        {
            throw new System.NotImplementedException();
        }

        public override string DropIndex(Index index)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterIndex(Index index)
        {
            throw new System.NotImplementedException();
        }


    }
}