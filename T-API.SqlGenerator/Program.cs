﻿using System;
using T_API.Entity.Concrete;
using T_API.SqlGenerator.Abstract;
using T_API.SqlGenerator.Concrete;

namespace T_API.SqlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Table table=new Table();
            //table.TableName = "Users";
            //table.Columns.Add(new Column()
            //{
            //    ColumnName = "UserId",
            //    NotNull = true,
            //    AutoInc = true,
            //    PrimaryKey = false,
            //    DataType = MysqlProviderColumnType.INT,

            //});
            //table.Columns.Add(new Column()
            //{
            //    ColumnName = "Firstname",
            //    NotNull = false,
            //    AutoInc = false,
            //    PrimaryKey = false,

            //    DataType = MysqlProviderColumnType.VARCHAR,
            //    HasLength = true,
            //    DataLength = 100

            //});
            //SqlCodeGenerator sqlCodeGenerator=new MySqlCodeGenerator();
            //sqlCodeGenerator.CreateTable(table);
            createTableMssql();
        }

        public static void createTableMssql()
        {
            Table table = new Table();
            table.TableName = "Users";
            table.Columns.Add(new Column()
            {
                ColumnName = "UserId",
                NotNull = false,
                AutoInc = false,        
                PrimaryKey = true,
                DataType = SqlServerProviderColumnType.INT,

            });
            
            table.Columns.Add(new Column()
            {
                ColumnName = "FirstName",
                NotNull = false,
                AutoInc = false,
                PrimaryKey = true,
                DataType = SqlServerProviderColumnType.NVARCHAR,
                DataLength = 50,
                HasLength = true

            });
            SqlCodeGenerator sqlCodeGenerator = new SqlServerCodeGenerator();
            sqlCodeGenerator.CreateTable(table);
        }
    }



}
