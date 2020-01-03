using System;
using T_API.Entity.Concrete;
using Index = T_API.Entity.Concrete.Index;

namespace T_API.BLL.Abstract
{
    public interface ISqlCodeGenerator : IDisposable
    {
        string GenerateCreateDatabaseQuery(Database database);
        string GenerateCreateTableQuery(Table table);
        string GenerateAddColumnQuery(Column column, Table table);
        string GenerateAddIndexQuery(Index index, Table table);
        string GenerateAddForeignKeyQuery(ForeignKey foreignKey, Table table);
        string GenerateAddKeyQuery(Key key, Table table);
        string GenerateModifyColumnQuery(Column column, Table table);
        string GenerateModifyIndexQuery(Index index, Table table);
        string GenerateModifyForeignKeyQuery(ForeignKey foreignKey, Table table);
        string GenerateModifyKeyQuery(Key key, Table table);
        string GenerateDropTableQuery(Table table);
        string GenerateDropColumnQuery(Column column, Table table);
        string GenerateDropRelationQuery(ForeignKey foreignKey, Table table);
        string GenerateDropKeyQuery(Key key, Table table);
        string GenerateDropIndexQuery(Index index, Table table);
    }
}