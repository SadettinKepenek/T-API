using System;
using System.Reflection;
using System.Text;
using T_API.Entity.Abstract;
using T_API.Entity.Concrete;
using T_API.SqlGenerator.Abstract;
using Index = T_API.Entity.Concrete.Index;

namespace T_API.SqlGenerator.Concrete
{
    public class MySqlCodeGenerator : SqlCodeGenerator
    {
        public override string CreateTable(Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Create Table '{table.TableName}'");
            sb.AppendLine("(");
            foreach (Column column in table.Columns)
            {
                string columnQuery = CreateColumn(column);
                sb.AppendLine(columnQuery);
            }
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
            if ((column.DataType.Equals("TEXT") || column.DataType.Equals("TINYTEXT") ||
                 column.DataType.Equals("CHAR") || column.DataType.Equals("VARCHAR") ||
                 column.DataType.Equals("LONGTEXT") || column.DataType.Equals("MEDIUMTEXT")))
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
            else if ((column.DataType.Equals("INT") ||
                    column.DataType.Equals("SMALLINT") || column.DataType.Equals("BIGINT") ||
                    column.DataType.Equals("FLOAT") || column.DataType.Equals("DOUBLE")
                    || column.DataType.Equals("DECIMAL")))
            {
                if ((column.HasLength || column.DataLength != 0))
                {
                    throw new ArgumentException("Column türü için size belirtilemez");
                }

                if (column.DefaultValue != null && (column.DefaultValue.GetType() != typeof(int) || column.DefaultValue.GetType() != typeof(double)
                                                    || column.DefaultValue.GetType() != typeof(decimal) || column.DefaultValue.GetType() != typeof(float)))
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
                    throw new ArgumentNullException("DefaultValue", "BOOL Veri tipi Auto Inc,Unique veya Primary Key olarak belirlenemez");
            }


            stringBuilder.Append($"\t{column.ColumnName} ");
            if (!String.IsNullOrEmpty(column.DataType)) stringBuilder.Append($"{column.DataType}");
            if (column.HasLength) stringBuilder.Append($"({column.DataLength})");
            else stringBuilder.Append(" ");

            if (column.DefaultValue != null)
                stringBuilder.Append($"default {column.DefaultValue}");



            if (column.AutoInc)
                if (column.DefaultValue == null)
                {
                    stringBuilder.Append(" auto_increment");
                }
                else
                {
                    throw new AmbiguousMatchException("Auto Increment mevcut iken Default value verilemez");
                }
            
            if (!column.PrimaryKey && column.NotNull ) 
                stringBuilder.Append(" not null");
            if (column.PrimaryKey)
                stringBuilder.Append(" primary key");

            stringBuilder.Append(',');

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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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