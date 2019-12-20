using System;
using T_API.Entity.Concrete;
using T_API.SqlGenerator.Abstract;
using T_API.SqlGenerator.Concrete;
using Index = T_API.Entity.Concrete.Index;

namespace T_API.SqlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table=new Table();
            table.TableName = "Users";
            table.Columns.Add(new Column()
            {
                ColumnName = "UserId",
                NotNull = true,
                AutoInc = true,
                PrimaryKey = true,
                DataType = MysqlProviderColumnType.INT,

            });
            table.Columns.Add(new Column()
            {
                ColumnName = "Firstname",
                NotNull = false,
                AutoInc = false,
                PrimaryKey = false,
                
                DataType = MysqlProviderColumnType.VARCHAR,
                HasLength = true,
                DataLength = 100

            });
            table.ForeignKeys.Add(new ForeignKey
            {
                ForeignKeyName = "FK_Databases_Users",
                TargetColumn = "UserId",
                SourceColumn = "UserId",
                TargetTable = "Users",
                SourceTable = "Databases",
                OnDeleteAction = "NULL",
                OnUpdateAction = "NULL"
            });
            table.ForeignKeys.Add(new ForeignKey
            {
                ForeignKeyName = "FK_Databases_Connections",
                TargetColumn = "DatabaseId",
                SourceColumn = "DatabaseId",
                TargetTable = "Connections",
                SourceTable = "Databases",
                OnDeleteAction = "NULL",
                OnUpdateAction = "NULL"
            });
            table.Indices.Add(new Index
            {
                IndexColumn = "Username",
                IndexName = "UK_Username",
                IsUnique = true,
                TableName = "Users",
                IndexOrder = "ASC",
                
            });
            table.Keys.Add(new Key
            {
               IsPrimary = false,
               KeyColumn = "Username",
               KeyName = "IX_Username"
            });
            table.Indices.Add(new Index
            {
                IndexColumn = "PhoneNumber",
                IndexName = "UK_Username",
                IsUnique = false,
                TableName = "Users",
                IndexOrder = "ASC",

            });
            SqlCodeGenerator sqlCodeGenerator=new MySqlCodeGenerator();
            sqlCodeGenerator.AlterTable(table);
        }

    
    }



}
