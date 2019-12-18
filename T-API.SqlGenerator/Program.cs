using System;
using T_API.Entity.Concrete;

namespace T_API.SqlGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Table table=new Table();
            table.Columns.Add(new Column()
            {
                ColumnName = "UserId",
                NotNull = true,
                AutoInc = true,
                
            });
        }

        string GenerateSql(OperationType operation,DataProvider provider)
        {
            if (provider==DataProvider.MySql)
            {
                return String.Empty;
            }

            if (provider==DataProvider.Sql)
            {
                return String.Empty;
            }
            return  String.Empty;
        }
    }



}
