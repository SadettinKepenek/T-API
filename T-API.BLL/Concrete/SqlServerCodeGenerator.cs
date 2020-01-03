using System;
using System.Reflection;
using System.Text;
using T_API.BLL.Abstract;
using T_API.Entity.Concrete;
using Index = T_API.Entity.Concrete.Index;

namespace T_API.BLL.Concrete
{
    public class SqlServerCodeGenerator : ISqlCodeGenerator
    {
      

        public void Dispose()
        {
        }

        public string GenerateCreateDatabaseQuery(Database database)
        {
            throw new NotImplementedException();
        }

        public string GenerateCreateTableQuery(Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateAddColumnQuery(Column column, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateAddIndexQuery(Index index, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateAddForeignKeyQuery(ForeignKey foreignKey, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateAddKeyQuery(Key key, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateModifyColumnQuery(Column column, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateModifyIndexQuery(Index index, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateModifyForeignKeyQuery(ForeignKey foreignKey, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateModifyKeyQuery(Key key, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateDropTableQuery(Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateDropColumnQuery(Column column, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateDropRelationQuery(ForeignKey foreignKey, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateDropKeyQuery(Key key, Table table)
        {
            throw new NotImplementedException();
        }

        public string GenerateDropIndexQuery(Index index, Table table)
        {
            throw new NotImplementedException();
        }
    }
}