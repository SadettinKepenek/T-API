﻿using T_API.Entity.Concrete;

namespace T_API.BLL.Abstract
{
    public interface  ISqlCodeGenerator
    {
         string CreateTable(Table table);
         string DropTable(Table table);
        string AlterTable(Table table);

        string CreateColumn(Column column);
        string DropColumn(Column column);
        string AlterColumn(Column column);

        string CreateRelation(ForeignKey foreignKey);
        string DropRelation(ForeignKey foreignKey);
        string AlterRelation(ForeignKey foreignKey);

        string CreateKey(Key key);
        string DropKey(Key key);
        string AlterKey(Key key);

        string CreateIndex(Index index);
        string DropIndex(Index index);
        string AlterIndex(Index index);
    }
}